using EventHorizon.Contracts.Responses;

namespace EventHorizon.Application.UseCases.Interfaces.Users
{
    public interface IGetAllUsersUseCase
    {
        Task<GetAllUsersResponse> ExecuteAsync(CancellationToken cancellationToken);
    }
}
