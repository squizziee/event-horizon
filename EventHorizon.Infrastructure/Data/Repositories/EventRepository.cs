using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public Task<PaginatedEnumerable<Event>> GetAllAsync(int chunkNumber, int chunkSize, CancellationToken cancellationToken)
        {
            if (chunkNumber < 0)
            {
                throw new ArgumentException($"Chunk number {chunkNumber} is unacceptable for pagination");
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentException($"Chunk size {chunkSize} is unacceptable for pagination");
            }

            var result = _context.Events
                .Include(e => e.Category)
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize)
                .AsEnumerable();

            var chunkCount = _context.Events.Count() / chunkSize + 1;

            if (_context.Events.Count() % chunkSize == 0) --chunkCount;

            if (chunkNumber >= chunkCount)
            {
                throw new ArgumentException($"Can't acquire {chunkNumber + 1}th chunk out of {chunkCount} chunks");
            }

            return Task.FromResult(
                new PaginatedEnumerable<Event>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
            );
        }

        public Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Events.AsEnumerable()
            );
        }

        public Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Events
                    .Include(e => e.Category)
                    .FirstOrDefault(u => u.Id == id)
            );
        }

        public Task<PaginatedEnumerable<Event>> GetFilteredAsync(
            Func<Event, bool> predicate, 
            int chunkNumber, 
            int chunkSize, 
            CancellationToken cancellationToken)
        {
            if (chunkNumber < 0)
            {
                throw new ArgumentException($"Chunk number {chunkNumber} is unacceptable for pagination");
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentException($"Chunk size {chunkSize} is unacceptable for pagination");
            }

            var filtered = _context.Events
                .Include(e => e.Category)
                .Where(predicate);

            var result = filtered
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize)
                .AsEnumerable();

            var chunkCount = filtered.Count() / chunkSize + 1;

            if (filtered.Count() % chunkSize == 0) --chunkCount;

            if (chunkNumber >= chunkCount)
            {
                throw new ArgumentException($"Can't acquire {chunkNumber + 1}th chunk out of {chunkCount} chunks");
            }

            return Task.FromResult(
                new PaginatedEnumerable<Event>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
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
