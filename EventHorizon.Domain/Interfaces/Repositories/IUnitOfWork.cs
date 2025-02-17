namespace EventHorizon.Domain.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IEventRepository Events { get; }
        IEventEntryRepository Entries { get; }
        IEventCategoryRepository Categories { get; }
        bool Save();
    }
}
