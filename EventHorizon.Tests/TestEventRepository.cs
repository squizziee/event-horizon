using AutoMapper;
using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Events;
using EventHorizon.Application.UseCases.Interfaces.Events;
using EventHorizon.Application.Validation;
using EventHorizon.Infrastructure.Data.Repositories;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Helpers;
using EventHorizon.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using EventHorizon.Domain.Entities;
using EventHorizon.Domain.Interfaces.Repositories;

namespace EventHorizon.Tests
{
    public class TestEventRepository
	{
		private DatabaseContext _context;
		private IUnitOfWork _unitOfWork;

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
				new ImageUploadOptions
				{
					Url = "uploads/img/events",
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
		public async void AddEvent_ShouldSucceed()
		{
			await FlushAndPopulateEvents(10);
			var categoryId = await AddCategory();

			var newEvent = new Event
			{
				Name = "test_event",
				Description = "test_event_desc",
				Address = "test_event_addr",
				DateTime = DateTime.Now.AddDays(77),
				MaxParticipantCount = 1000 + 77,
				CategoryId = categoryId,
			};

			Assert.True(_context.Events.Count() == 10);

			await _unitOfWork.Events.AddAsync(newEvent, CancellationToken.None);
			_unitOfWork.Save();

			Assert.True(_context.Events.Count() == 11);
		}

		[Fact]
		public async void GetEvent_ShouldSucceed()
		{
			await FlushAndPopulateEvents(10);

			var categoryId = await AddCategory();

			var eventId = await AddEvent(99, categoryId);

			var event_ = await _unitOfWork.Events.GetByIdAsync(eventId, CancellationToken.None);

			Assert.NotNull(event_);
			Assert.True(event_.MaxParticipantCount == 1000 + 99);
		}

        [Fact]
        public async void GetEvent_NotExistentId_ShouldThrow()
        {
            await FlushAndPopulateEvents(10);

            var event_ = await _unitOfWork.Events.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

            Assert.Null(event_);
        }

        [Fact]
        public async void GetAllEvents_ShouldReturnAll()
        {
            await FlushAndPopulateEvents(10);

            var events = await _unitOfWork.Events.GetAllAsync(CancellationToken.None);

            Assert.NotNull(events);
            Assert.True(events.Count() == 10);
        }

        [Fact]
        public async void GetAllEvents_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var events = await _unitOfWork.Events.GetAllAsync(0, 3, CancellationToken.None);

            Assert.NotNull(events);
            Assert.True(events.Items.Count() == 3);
            Assert.True(events.ChunkSequenceNumber == 0);
            Assert.True(events.TotalChunkCount == 4);
        }

		[Fact]
		public async void GetAllEvents_InvalidChunkNumber_ShouldThrow()
        {
            await FlushAndPopulateEvents(10);

			PaginatedEnumerable<Event> events;

            var exception = await Record.ExceptionAsync(
                async () => events = await _unitOfWork.Events.GetAllAsync(765, 3, CancellationToken.None)
            );

            Assert.True(exception is ArgumentException);
        }

        [Fact]
        public async void GetAllEvents_InvalidChunkSize_ShouldThrow()
        {
            await FlushAndPopulateEvents(10);

            PaginatedEnumerable<Event> events;

            var exception = await Record.ExceptionAsync(
                async () => events = await _unitOfWork.Events.GetAllAsync(0, 0, CancellationToken.None)
            );

            Assert.True(exception is ArgumentException);
        }

        [Fact]
        public async void GetFilteredEvents_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

			var predicate = (Event e) => e.Address.Contains("test");

            var events = await _unitOfWork.Events.GetFilteredAsync(
				predicate,
				0, 3, CancellationToken.None
			);

            Assert.NotNull(events);
            Assert.True(events.Items.Count() == 3);
            Assert.True(events.ChunkSequenceNumber == 0);
            Assert.True(events.TotalChunkCount == 4);
        }

        [Fact]
        public async void GetFilteredEvents2_ShouldReturnPaginated()
        {
            await FlushAndPopulateEvents(10);

            var predicate = (Event e) => e.MaxParticipantCount > 1003 && e.MaxParticipantCount < 1008;

            var events = await _unitOfWork.Events.GetFilteredAsync(
                predicate,
                0, 3, CancellationToken.None
            );

            Assert.NotNull(events);
            Assert.True(events.Items.Count() == 3);
            Assert.True(events.ChunkSequenceNumber == 0);
            Assert.True(events.TotalChunkCount == 2);
        }
    }
}
