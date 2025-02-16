using EventHorizon.Contracts.Requests;

namespace EventHorizon.Application.UseCases.Interfaces.Users
{
    public interface IRegisterUserUseCase
    {
        Task<(string, string)?> ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken);
    }
}
