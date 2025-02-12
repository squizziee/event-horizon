using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;
using System;

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

        public Task<PaginatedEnumerable<EventCategory>> GetAllAsync(int chunkNumber, int chunkSize, CancellationToken cancellationToken)
        {
            if (chunkNumber < 0)
            {
                throw new ArgumentException($"Chunk number {chunkNumber} is unacceptable for pagination");
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentException($"Chunk size {chunkSize} is unacceptable for pagination");
            }

            var result = _context.EventCategories
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize)
                .AsEnumerable();

            var chunkCount = _context.EventCategories.Count() / chunkSize + 1;

            if (_context.EventCategories.Count() % chunkSize == 0) --chunkCount;

            if (chunkNumber >= chunkCount)
            {
                throw new ArgumentException($"Can't acquire {chunkNumber + 1}th chunk out of {chunkCount} chunks");
            }

            return Task.FromResult(
                new PaginatedEnumerable<EventCategory>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
            );
        }

        public Task<EventCategory?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.EventCategories.FirstOrDefault(u => u.Id == id)
            );
        }

        public Task<PaginatedEnumerable<EventCategory>> GetFilteredAsync(
            Func<EventCategory, bool> predicate, 
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

            var filtered = _context.EventCategories.Where(predicate);

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
                new PaginatedEnumerable<EventCategory>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
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
