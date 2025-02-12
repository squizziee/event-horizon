using EventHorizon.Contracts.Requests.EventCategories;
using EventHorizon.Contracts.Responses.EventCategories;

namespace EventHorizon.Application.UseCases.Interfaces.EventCategories
{
    public interface IGetAllCategoriesUseCase
    {
        Task<GetAllCategoriesResponse> ExecuteAsync(GetAllCategoriesRequest request, CancellationToken cancellationToken);
    }
}
