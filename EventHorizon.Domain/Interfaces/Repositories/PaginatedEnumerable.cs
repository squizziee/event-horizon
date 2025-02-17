namespace EventHorizon.Domain.Interfaces.Repositories
{
    public record PaginatedEnumerable<TEntity>
    {
        public required IEnumerable<TEntity> Items { get; set; }
        public required int ChunkSequenceNumber { get; set; }
        public required int TotalChunkCount { get; set; }
    }
}
