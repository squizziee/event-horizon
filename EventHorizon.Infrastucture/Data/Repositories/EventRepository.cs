using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseContext _context;

        public EventRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Event entity, CancellationToken cancellationToken)
        {
            await _context.Events.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(Event entity, CancellationToken cancellationToken)
        {
            _context.Events.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Events.FirstOrDefault(u => u.Id == id)
            );
        }

        public async Task UpdateAsync(Event entity, CancellationToken cancellationToken)
        {
            var tryFind = _context.Events.FirstOrDefault(u => u.Id == entity.Id);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No event with id {entity.Id} was found");
            }

            tryFind.Name = entity.Name;
            tryFind.Description = entity.Description;
            tryFind.Address = entity.Address;
            tryFind.CategoryId = entity.CategoryId;
            tryFind.DateTime = entity.DateTime;
            tryFind.MaxParticipantCount = entity.MaxParticipantCount;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
