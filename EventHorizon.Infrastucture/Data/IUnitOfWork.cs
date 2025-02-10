using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data
{
    public interface IUnitOfWork
    {
       IUserRepository Users { get; }
       IEventRepository Events { get; }
       IEventEntryRepository Entries { get; }
       IEventCategoryRepository Categories { get; }
    }
}
