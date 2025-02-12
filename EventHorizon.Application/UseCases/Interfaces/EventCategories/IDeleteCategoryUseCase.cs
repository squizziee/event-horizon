namespace EventHorizon.Application.UseCases.Interfaces.EventCategories
{
    public interface IDeleteCategoryUseCase
    {
        Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
