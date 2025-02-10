using EventHorizon.Application.UseCases.Interfaces;
using EventHorizon.Contracts.Requests;

namespace EventHorizon.Application.UseCases
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        public Task<(string, string)> ExecuteAsync(RegsiterUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
