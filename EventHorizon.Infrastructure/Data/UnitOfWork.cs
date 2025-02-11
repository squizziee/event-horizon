using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly DatabaseContext _context;	
		public IUserRepository Users {  get; set; }
		public IEventRepository Events {  get; set; }
		public IEventEntryRepository Entries {  get; set; }
		public IEventCategoryRepository Categories {  get; set; }

		public UnitOfWork(
			DatabaseContext context,
			IUserRepository userRepository,
			IEventRepository eventRepository,
			IEventEntryRepository eventEntryRepository,
			IEventCategoryRepository eventCategoryRepository) {
			_context = context;
			Users = userRepository;
			Events = eventRepository;
			Entries = eventEntryRepository;
			Categories = eventCategoryRepository;
		}

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
