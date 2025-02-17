using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.MapperProfiles;
using EventHorizon.Application.UseCases.Events;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Application.Validation;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Domain.Entities;
using EventHorizon.Domain.Interfaces.Repositories;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Data.Repositories;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services;
using FluentValidation;
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

        private IGetAllEventsUseCase _getAllEventsUseCase;
        private IGetEventUseCase _getEventUseCase;
        private ISearchEventsUseCase _searchEventsUseCase;
        private IAddEventUseCase _addEventUseCase;
        private IUpdateEventUseCase _updateEventUseCase;
        private IDeleteEventUseCase _deleteEventUseCase;
        private IMapper _mapper;

        private AddEventRequestValidator _addEventRequestValidator = new();
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
            _getAllEventsUseCase = new GetAllEventsUseCase(_unitOfWork, _mapper, paginationOptions);
            _getEventUseCase = new GetEventUseCase(_unitOfWork, _mapper);
            _searchEventsUseCase = new SearchEventsUseCase(_unitOfWork, _mapper, paginationOptions);
            _addEventUseCase = new AddEventUseCase(_unitOfWork, imageService, _addEventRequestValidator, _mapper);
            _updateEventUseCase = new UpdateEventUseCase(_unitOfWork, imageService, _updateEventRequestValidator, _mapper);
            _deleteEventUseCase = new DeleteEventUseCase(_unitOfWork, imageService);
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

            var categoryId = await AddCategory();

            for (int i = 0; i < eventCount; i++)
            {
                var event_ = new Event
                {
                    Name = $"test_event_{i}",
                    Description = $"test_event_desc_{i}",
                    Address = $"test_event_addr_{i}",
                    DateTime = DateTime.Now.AddDays(i),
                    MaxParticipantCount = 1000 + i,
                    CategoryId = categoryId,
                };

                _context.Add(event_);
            }
            _context.SaveChanges();

            return categoryId;
        }

        private async Task<Guid> AddCategory()
        {
            var id = Guid.NewGuid();
            _context.EventCategories.Add(new EventCategory
            {
                Id = id,
                Name = "test_category",
                Description = "test_category_description",
            });
            _context.SaveChanges();
            return id;
        }

        private async Task<Guid> AddEvent(int i, Guid categoryId)
        {
            var id = Guid.NewGuid();
            _context.Events.Add(new Event
            {
                Id = id,
                Name = $"test_event_{i}",
                Description = $"test_event_desc_{i}",
                Address = $"test_event_addr_{i}",
                DateTime = DateTime.Now.AddDays(i),
                MaxParticipantCount = 1000 + i,
                CategoryId = categoryId,
            });
            _context.SaveChanges();
            return id;
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

        [Fact]
        public async Task GetEvent_ShouldReturnDTO()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(23, categoryId);

            var eventResponse = await _getEventUseCase.ExecuteAsync(eventId, CancellationToken.None);

            Assert.True(eventResponse.Event.Id == eventId);
            Assert.True(eventResponse.Event.MaxParticipantCount == 1000 + 23);
        }

        [Fact]
        public async Task GetEvent_NonExistentId_ShouldThrow()
        {
            await FlushAndPopulateEvents(10);

            await Assert.ThrowsAsync<ResourceNotFoundException>(
                async () => await _getEventUseCase.ExecuteAsync(Guid.NewGuid(), CancellationToken.None)
            );
        }

        [Fact]
        public async Task AddEvent_ShouldSucceed()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "test_event_desc",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 1000 + 77,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.Null(exception);
            Assert.True(_context.Events.Count() == 11);
        }

        [Fact]
        public async Task AddEvent_InvalidName_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "",
                Description = "test_event_desc",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 1000 + 77,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ValidationException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task AddEvent_InvalidDescription_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 1000 + 77,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ValidationException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task AddEvent_InvalidAddress_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "test_event_desc",
                Address = "",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 1000 + 77,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ValidationException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task AddEvent_InvalidDate_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "test_event_desc",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                MaxParticipantCount = 1000 + 77,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ValidationException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task AddEvent_InvalidMaxParticipantCount_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "test_event_desc",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 0,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ValidationException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task AddEvent_NonExistentCategoryId_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var addEventRequest = new AddEventRequest
            {
                Name = "test_event",
                Description = "test_event_desc",
                Address = "test_event_addr",
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(77)),
                Time = TimeOnly.FromDateTime(DateTime.Now.AddDays(77)),
                MaxParticipantCount = 1000 + 77,
                CategoryId = Guid.NewGuid(),
            };

            var exception = await Record.ExceptionAsync(async () => await _addEventUseCase.ExecuteAsync(addEventRequest, CancellationToken.None));

            Assert.True(exception is ResourceNotFoundException);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task UpdateEvent_ShouldSucceed()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(99);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "update",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 23,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.Null(exception);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name == "update");
            Assert.True(supposedToChange.Description == "update");
            Assert.True(supposedToChange.Address == "update");
            Assert.True(supposedToChange.DateTime == time);
            Assert.True(supposedToChange.MaxParticipantCount == 23);
        }

        [Fact]
        public async Task UpdateEvent_InvalidName_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(99);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "",
                Description = "update",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 23,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task UpdateEvent_InvalidDescription_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(99);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 23,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task UpdateEvent_InvalidAddress_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(99);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "update",
                Address = "",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 23,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task UpdateEvent_InvalidDate_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now;

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "update",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 23,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task UpdateEvent_InvalidMaxParticipantCount_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(77);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "update",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 0,
                CategoryId = categoryId,
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task UpdateEvent_NonExistentCategoryId_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            var time = DateTime.Now.AddDays(77);

            var updateEventRequest = new UpdateEventRequest
            {
                Name = "update",
                Description = "update",
                Address = "update",
                Date = DateOnly.FromDateTime(time),
                Time = TimeOnly.FromDateTime(time),
                MaxParticipantCount = 0,
                CategoryId = Guid.NewGuid(),
            };

            var exception = await Record.ExceptionAsync(
                async () => await _updateEventUseCase.ExecuteAsync(eventId, updateEventRequest, CancellationToken.None)
            );

            Assert.True(exception is ValidationException);

            var supposedToChange = _context.Events.Where(e => e.Id == eventId).First();

            Assert.True(supposedToChange.Name != "update");
            Assert.True(supposedToChange.Description != "update");
            Assert.True(supposedToChange.Address != "update");
            Assert.True(supposedToChange.DateTime != time);
            Assert.True(supposedToChange.MaxParticipantCount != 23);
        }

        [Fact]
        public async Task DeleteEvent_ShouldSucceed()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            Assert.True(_context.Events.Count() == 11);

            var exception = await Record.ExceptionAsync(
                async () => await _deleteEventUseCase.ExecuteAsync(eventId, CancellationToken.None)
            );

            Assert.Null(exception);
            Assert.True(_context.Events.Count() == 10);
        }

        [Fact]
        public async Task DeleteEvent_NonExistentId_ShouldThrow()
        {
            var categoryId = await FlushAndPopulateEvents(10);

            var eventId = await AddEvent(997, categoryId);

            Assert.True(_context.Events.Count() == 11);

            var exception = await Record.ExceptionAsync(
                async () => await _deleteEventUseCase.ExecuteAsync(Guid.NewGuid(), CancellationToken.None)
            );

            Assert.True(exception is ResourceNotFoundException);
            Assert.True(_context.Events.Count() == 11);
        }

    }
}