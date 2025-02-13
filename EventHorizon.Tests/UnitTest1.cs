using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.MapperProfiles;
using EventHorizon.Application.UseCases.EventCategories;
using EventHorizon.Application.UseCases.Events;
using EventHorizon.Application.UseCases.Interfaces.EventCategories;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Application.Validation;
using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Data.Repositories;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;

namespace EventHorizon.Tests
{
    public class TestEventUseCases
	{
		private DatabaseContext _context;
		private IUnitOfWork _unitOfWork;

        private IAddCategoryUseCase _addCategoryUseCase;
        private IGetAllCategoriesUseCase _getCategoriesUseCase;

        private IGetAllEventsUseCase _getAllEventsUseCase;
        private IGetEventUseCase _getEventUseCase;
        private ISearchEventsUseCase _searchEventsUseCase;
        private IAddEventUseCase _addEventUseCase;
        private IUpdateEventUseCase _updateEventUseCase;
        private IDeleteEventUseCase _deleteEventUseCase;
        private IMapper _mapper;

        private AddEventRequestValidator _addEventRequestValidator = new();
        private AddCategoryRequestValidator _addCategoryRequestValidator = new();
        private UpdateEventRequestValidator _updateEventRequestValidator = new();

        private Mock<IWebHostEnvironment> _environment;

		private void Init()
		{
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var myConfiguration = new Dictionary<string, string>
            {
                {"Key1", "Value1"},
                {"Nested:Key1", "NestedValue1"},
                {"Nested:Key2", "NestedValue2"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration!)
                .Build();

            var paginationOptions = Options.Create(new PaginationOptions { PageSize = 3 });
            var imageOptions = Options.Create(
                new ImageUploadOptions { 
                    Url = "uploads/img/events" ,
                    SupportedExtensions = [
                        "jpg", "jpeg", "png"
                    ],
                    AccessibleUrl = "https://localhost:8081"
                }
            );

            _context = new DatabaseContext(builder.Options);

            _unitOfWork = new UnitOfWork(
                _context,
                new UserRepository(_context),
                new EventRepository(_context),
                new EventEntryRepository(_context),
                new EventCategoryRepository(_context)
            );

            var mapperConfig = new MapperConfiguration(
                mc =>
                {
                    mc.AddProfile(new UserMapperProfile());
                    mc.AddProfile(new EventMapperProfile());
                    mc.AddProfile(new CategoryMapperProfile());
                    mc.AddProfile(new EventRequestToEntityMapperProfile());
                    mc.AddProfile(new EventEntryMapperProfile());
                    mc.AddProfile(new UserEventEntryMapperProfile());
                }
            );

            _mapper = mapperConfig.CreateMapper();

            var imageService = new ImageService(imageOptions, _environment.Object);

            // here we go baby
            _addCategoryUseCase = new AddCategoryUseCase(_unitOfWork, _addCategoryRequestValidator);
            _getCategoriesUseCase = new GetAllCategoriesUseCase(_unitOfWork, paginationOptions, _mapper);

            _getAllEventsUseCase = new GetAllEventsUseCase(_unitOfWork, _mapper, paginationOptions);
            _getEventUseCase = new GetEventUseCase(_unitOfWork, _mapper);
            _searchEventsUseCase = new SearchEventsUseCase(_unitOfWork, _mapper, paginationOptions);
            _addEventUseCase = new AddEventUseCase(_unitOfWork, imageService, _addEventRequestValidator, _mapper);
        }

        public TestEventUseCases ()
		{
            _environment = new();
            _environment
                .Setup(x => x.WebRootPath)
                .Returns("/app/wwwroot");

            Init(); 
		}

		private async Task<Guid> FlushAndPopulateEvents(int eventCount)
		{
            Init();

            var addCategoryRequest = new AddCategoryRequest
            {
                Name = "test_category",
                Description = "test_category_description",
            };


            await _addCategoryUseCase.ExecuteAsync(addCategoryRequest, CancellationToken.None);

            var getCategoriesRequest = new GetAllCategoriesRequest { 
                PageNumber = 0 
            };

            var categories = await _getCategoriesUseCase.ExecuteAsync(getCategoriesRequest, CancellationToken.None);

            var categoryId = categories.Categories.First().Id;

            for (int i = 0; i < eventCount; i++)
            {
                var addEventRequest = new AddEventRequest
                {
                    Name = $"test_event_{i}",
                    Description = $"test_event_desc_{i}",
                    Address = $"test_event_addr_{i}",
                    DateTime = DateTime.Now.AddDays(i),
                    MaxParticipantCount = 1000 + i,
                    CategoryId = categoryId,
                };

                await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None);
            }

            return categoryId;
        }

		[Fact]
		public async void GetAllEvents_ShouldReturnPaginated()
		{
            await FlushAndPopulateEvents(10);

			var request = new GetAllEventsRequest { 
				PageNumber = 1 
			};

			var events = await _getAllEventsUseCase.ExecuteAsync(request, CancellationToken.None);

			Assert.True(events.TotalPages == 4);
			Assert.True(events.PageNumber == 1);
			Assert.True(events.Events.Count() == 3);
		}

        [Fact]
        public async Task GetAllEvents_PageNumberInvalid_ShouldThrow()
        {
            await FlushAndPopulateEvents(10);

            var request = new GetAllEventsRequest
            {
                PageNumber = 24
            };

            await Assert.ThrowsAsync<ArgumentException>(
                async () => await _getAllEventsUseCase.ExecuteAsync(request, CancellationToken.None)
            );
        }

        [Fact]
        public async Task SearchEvents_NoFilters_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                PageNumber = 1
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 4);
            Assert.True(events.PageNumber == 1);
            Assert.True(events.Events.Count() == 3);
        }

        [Fact]
        public async Task SearchEvents_TextQueryFilter_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                TextQuery = "2",
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 1);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 1);
        }

        [Fact]
        public async Task SearchEvents_PlaceQueryFilter_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                PlaceQuery = "3",
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 1);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 1);
        }

        [Fact]
        public async Task SearchEvents_SearchFromDateFilter_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                SearchFromDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 3);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 3);
        }

        [Fact]
        public async Task SearchEvents_SearchUntilDateFilter_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                SearchUntilDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 1);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 2);
        }

        [Fact]
        public async Task SearchEvents_CategoriesFilter_ShouldReturnPaginated()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                Categories = [categoryId],
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 4);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 3);
        }

        [Fact]
        public async Task SearchEvents_CategoriesFilter_NonExistentCategory_ShouldThrow()
        {
            var request = new SearchEventsRequest
            {
                Categories = [Guid.NewGuid()],
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 0);
            Assert.True(events.PageNumber == 0);
            Assert.True(!events.Events.Any());
        }

        [Fact]
        public async Task SearchEvents_AllFilters_ShouldReturnPaginated()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var request = new SearchEventsRequest
            {
                TextQuery = "desc",
                PlaceQuery = "addr",
                SearchFromDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                SearchUntilDate = DateOnly.FromDateTime(DateTime.Now.AddDays(5)),
                Categories = [categoryId],
                PageNumber = 0
            };

            var events = await _searchEventsUseCase.ExecuteAsync(request, CancellationToken.None);

            Assert.True(events.TotalPages == 1);
            Assert.True(events.PageNumber == 0);
            Assert.True(events.Events.Count() == 3);
        }
    }
}