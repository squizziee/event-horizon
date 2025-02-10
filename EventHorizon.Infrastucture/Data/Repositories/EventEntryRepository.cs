using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data.Repositories
{
    public class EventEntryRepository : IEventEntryRepository
    {
        private readonly DatabaseContext _context;

        public EventEntryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(EventEntry entity, CancellationToken cancellationToken)
        {
            await _context.EventEntries.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(EventEntry entity, CancellationToken cancellationToken)
        {
            _context.EventEntries.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<EventEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.EventEntries.FirstOrDefault(u => u.Id == id)
            );
        }

        public Task UpdateAsync(EventEntry entity, CancellationToken cancellationToken)
        {
            throw new ImmutableResourceException("There is no merit in editing entries");
        }
    }
}
