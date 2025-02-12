using EventHorizon.Contracts.Responses.EventCategories;

namespace EventHorizon.Application.UseCases.Interfaces.EventCategories
{
    public interface IGetCategoryUseCase
    {
        Task<GetCategoryResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
