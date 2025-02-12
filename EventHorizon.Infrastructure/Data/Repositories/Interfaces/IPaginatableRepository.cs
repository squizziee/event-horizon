namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{
    public interface IPaginatableRepository<TEntity>
    {
        Task<PaginatedEnumerable<TEntity>> GetAllAsync(int chunkNumber, int chunkSize, CancellationToken cancellationToken);
        Task<PaginatedEnumerable<TEntity>> GetFilteredAsync(
            Func<TEntity, bool> predicate, 
            int chunkNumber, 
            int chunkSize, 
            CancellationToken cancellationToken);
    }
}
