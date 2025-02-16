using EventHorizon.Application.Helpers;
using EventHorizon.Application.UseCases.Interfaces.Dev;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data;
using EventHorizon.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace EventHorizon.Application.UseCases.Dev
{
	public class CreateAdminUseCase : ICreateAdminUseCase
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPasswordService _passwordService;
		private readonly AdminCredentials _adminCredentials;

		public CreateAdminUseCase(
			IUnitOfWork unitOfWork,
			IPasswordService passwordService,
			IOptions<AdminCredentials> adminCredentials)
		{
			_unitOfWork = unitOfWork;
			_passwordService = passwordService;
			_adminCredentials = adminCredentials.Value;
		}
		public async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			var newAdmin = new User
			{
				Email = _adminCredentials.Email,
				FirstName = _adminCredentials.FirstName,
				LastName = _adminCredentials.LastName,
				PasswordHash = _passwordService.HashPassword(_adminCredentials.Password),
				Role = Domain.Enums.UserRole.Admin,
				DateOfBirth = DateOnly.FromDateTime(
					new DateTime(1980, 1, 1, 0, 0, 0)
				)
			};

			await _unitOfWork.Users.AddAsync(newAdmin, cancellationToken);
			_unitOfWork.Save();
		}
	}
}
