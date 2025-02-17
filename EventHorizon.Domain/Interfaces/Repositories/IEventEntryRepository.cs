using EventHorizon.Domain.Entities;

namespace EventHorizon.Domain.Interfaces.Repositories
{
    public interface IEventEntryRepository : IRepository<EventEntry>, IPaginatableRepository<EventEntry>
    {
        Task<IEnumerable<EventEntry>> GetByUserIdWithEventAsync(Guid id, CancellationToken cancellationToken);
    }
}
