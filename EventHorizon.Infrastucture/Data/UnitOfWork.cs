using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		public IUserRepository Users {  get; set; }
		public IEventRepository Events {  get; set; }
		public IEventEntryRepository Entries {  get; set; }
		public IEventCategoryRepository Categories {  get; set; }

		public UnitOfWork(
			IUserRepository userRepository,
			IEventRepository eventRepository,
			IEventEntryRepository eventEntryRepository,
			IEventCategoryRepository eventCategoryRepository) {
			Users = userRepository;
			Events = eventRepository;
			Entries = eventEntryRepository;
			Categories = eventCategoryRepository;
		}
	}
}
