using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.Exceptions;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Services.Interfaces;

namespace EventHorizon.Application.UseCases.Users
{
    public class RefreshTokensUseCase : IRefreshTokensUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public RefreshTokensUseCase(
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<(string, string)?> ExecuteAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var tryFind = await _unitOfWork.Users.FindByRefreshToken(refreshToken);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No user with provided token was found");
            }

            var tokens = (
                _tokenService.GenerateAccessToken(tryFind),
                _tokenService.GenerateRefreshToken(tryFind)
            );

            tryFind.RefreshToken = tokens.Item2;
            await _unitOfWork.Users.UpdateAsync(tryFind, cancellationToken);

            return (tokens.Item1, tokens.Item2);
        }
    }
}
