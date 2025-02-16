using EventHorizon.Infrastructure.Data.Repositories.Interfaces;
using System.Transactions;

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
            var tr = _context.Database.BeginTransaction();
            try
			{
				_context.SaveChanges();
                tr.Commit();
            } catch
			{
				tr.Rollback();
			}

            //tr.Dispose();

        }
    }
}
