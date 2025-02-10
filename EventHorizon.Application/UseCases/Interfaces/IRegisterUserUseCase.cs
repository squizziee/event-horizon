using EventHorizon.Contracts.Requests;

namespace EventHorizon.Application.UseCases.Interfaces
{
    public interface IRegisterUserUseCase
    {
        Task<(string, string)> ExecuteAsync(RegsiterUserRequest request);
    }
}
