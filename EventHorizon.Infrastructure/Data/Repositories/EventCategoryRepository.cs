using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data.Repositories
{
    public class EventCategoryRepository : IEventCategoryRepository
    {
        private readonly DatabaseContext _context;

        public EventCategoryRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task AddAsync(EventCategory entity, CancellationToken cancellationToken)
        {
            await _context.EventCategories.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(EventCategory entity, CancellationToken cancellationToken)
        {
            _context.EventCategories.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<EventCategory>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.EventCategories.AsEnumerable()
            );
        }

        public Task<EventCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.EventCategories.FirstOrDefault(u => u.Id == id)
            );
        }

        public async Task UpdateAsync(EventCategory entity, CancellationToken cancellationToken)
        {
            var tryFind = _context.EventCategories.FirstOrDefault(u => u.Id == entity.Id);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No category with id {entity.Id} was found");
            }

            tryFind.Name = entity.Name;
            tryFind.Description = entity.Description;


            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
