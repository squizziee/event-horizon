using EventHorizon.Contracts.Responses;

namespace EventHorizon.Application.UseCases.Interfaces
{
    public interface IGetAllUsersUseCase
    {
        Task<GetAllUsersResponse> ExecuteAsync(CancellationToken cancellationToken);
    }
}
