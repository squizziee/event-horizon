using EventHorizon.Contracts.Requests.EventCategories;

namespace EventHorizon.Application.UseCases.Interfaces.EventCategories
{
    public interface IUpdateCategoryUseCase
    {
        Task ExecuteAsync(Guid id, UpdateCategoryRequest request, CancellationToken cancellationToken);
    }
}
