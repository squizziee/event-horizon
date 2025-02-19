using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public Task<IEnumerable<EventEntry>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.EventEntries
                    .Include(ee => ee.User)
                    .AsEnumerable()
            );
        }

        public Task<PaginatedEnumerable<EventEntry>> GetAllAsync(int chunkNumber, int chunkSize, CancellationToken cancellationToken)
        {
            if (chunkNumber < 0)
            {
                throw new ArgumentException($"Chunk number {chunkNumber} is unacceptable for pagination");
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentException($"Chunk size {chunkSize} is unacceptable for pagination");
            }

            var result = _context.EventEntries
                .Include(e => e.User)
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize)
                .AsEnumerable();

            var chunkCount = _context.EventEntries.Count() / chunkSize + 1;

            if (_context.EventEntries.Count() % chunkSize == 0) --chunkCount;

            if (!_context.EventEntries.Any())
            {
                return Task.FromResult(
                    new PaginatedEnumerable<EventEntry>
                    {
                        ChunkSequenceNumber = 0,
                        TotalChunkCount = 0,
                        Items = []
                    }
                );
            }

            if (chunkNumber >= chunkCount)
            {
                throw new ArgumentException($"Can't acquire {chunkNumber + 1}th chunk out of {chunkCount} chunks");
            }

            return Task.FromResult(
                new PaginatedEnumerable<EventEntry>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
            );
        }

        public Task<EventEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = _context.EventEntries
                .Include(e => e.User)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return result;
        }

        public Task<PaginatedEnumerable<EventEntry>> GetFilteredAsync(
            Func<EventEntry, bool> predicate, 
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

            var filtered = _context.EventEntries
                .Include(e => e.User)
                .Where(predicate);

            var result = filtered
                .Skip(chunkNumber * chunkSize)
                .Take(chunkSize)
                .AsEnumerable();

            var chunkCount = filtered.Count() / chunkSize + 1;

            if (filtered.Count() % chunkSize == 0) --chunkCount;

            if (!filtered.Any())
            {
                return Task.FromResult(
                    new PaginatedEnumerable<EventEntry>
                    {
                        ChunkSequenceNumber = 0,
                        TotalChunkCount = 0,
                        Items = []
                    }
                );
            }

            if (chunkNumber >= chunkCount)
            {
                throw new ArgumentException($"Can't acquire {chunkNumber + 1}th chunk out of {chunkCount} chunks");
            }

            return Task.FromResult(
                new PaginatedEnumerable<EventEntry>
                {
                    ChunkSequenceNumber = chunkNumber,
                    TotalChunkCount = chunkCount,
                    Items = result
                }
            );
        }

        public Task UpdateAsync(EventEntry entity, CancellationToken cancellationToken)
        {
            throw new ImmutableResourceException("There is no merit in editing entries");
        }

        public Task<IEnumerable<EventEntry>> GetByUserIdWithEventAsync(Guid userId, CancellationToken cancellationToken)
        {
            var result = _context.EventEntries
                .Include(ee => ee.Event)
                .Where(u => u.UserId == userId)
                .AsEnumerable();

            return Task.FromResult(result);
        }
    }
}
