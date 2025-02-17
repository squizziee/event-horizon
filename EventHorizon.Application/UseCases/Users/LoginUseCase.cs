using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Contracts.Requests;
using EventHorizon.Domain.Interfaces.Repositories;

using EventHorizon.Infrastructure.Services.Interfaces;

namespace EventHorizon.Application.UseCases.Users
{
    public class LoginUseCase : ILoginUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public LoginUseCase(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<(string, string)?> ExecuteAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Users.FindByEmail(request.Email);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No user with email {request.Email} was found");
            }

            if (!_passwordService.VerifyPassword(request.Password, tryFind.PasswordHash))
            {
                throw new InvalidCredentialException($"Invalid password for {request.Email} was provided");
            }

            var tokens = (
               _tokenService.GenerateAccessToken(tryFind),
               _tokenService.GenerateRefreshToken(tryFind)
            );

            tryFind.RefreshToken = tokens.Item2;
            await _unitOfWork.Users.UpdateAsync(tryFind, cancellationToken);

            return tokens;
        }
    }
}
