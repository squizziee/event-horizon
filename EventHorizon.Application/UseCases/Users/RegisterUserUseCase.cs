using EventHorizon.Application.UseCases.Interfaces.Users;
using EventHorizon.Contracts.Requests;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Services.Interfaces;
using FluentValidation;

namespace EventHorizon.Application.UseCases.Users
{
    public class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IValidator<RegisterUserRequest> _validator;

        public RegisterUserUseCase(
            IUnitOfWork unitOfWork,
            ITokenService tokenService,
            IPasswordService passwordService,
            IValidator<RegisterUserRequest> validator)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _validator = validator;
        }

        // no validation for email existence is done because email PGSQL index was created,
        // so it will just throw on duplicate email
        public async Task<(string, string)?> ExecuteAsync(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request);

            var newUser = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DateOfBirth = request.DateOfBirth,
                PasswordHash = _passwordService.HashPassword(request.Password)
            };

            try
            {
                await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
                _unitOfWork.Save();
            }
            catch (Exception)
            {
                return null;
            }

            var tokens = (
               _tokenService.GenerateAccessToken(newUser),
               _tokenService.GenerateRefreshToken(newUser)
            );

            newUser.RefreshToken = tokens.Item2;
            await _unitOfWork.Users.UpdateAsync(newUser, cancellationToken);

            return tokens;

        }
    }
}
