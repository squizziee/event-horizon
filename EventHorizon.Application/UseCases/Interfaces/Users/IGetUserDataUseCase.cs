using EventHorizon.Contracts.Responses;

namespace EventHorizon.Application.UseCases.Interfaces.Users
{
    public interface IGetUserDataUseCase
    {
        Task<GetUserDataResponse> ExecuteAsync(Guid Id, CancellationToken cancellationToken);
    }
}
