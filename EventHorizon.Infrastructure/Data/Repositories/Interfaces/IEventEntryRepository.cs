using EventHorizon.Domain.Entities;

namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{
    public interface IEventEntryRepository : IRepository<EventEntry>, IPaginatableRepository<EventEntry>
    {
        Task<IEnumerable<EventEntry>> GetByUserIdWithEventAsync(Guid id, CancellationToken cancellationToken);
    }
}
