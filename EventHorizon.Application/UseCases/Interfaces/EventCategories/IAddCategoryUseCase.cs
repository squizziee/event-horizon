using EventHorizon.Contracts.Requests.EventCategories;

namespace EventHorizon.Application.UseCases.Interfaces.EventCategories
{
    public interface IAddCategoryUseCase
    {
        Task ExecuteAsync(AddCategoryRequest request, CancellationToken cancellationToken);
    }
}
