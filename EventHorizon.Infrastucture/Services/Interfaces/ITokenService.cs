using EventHorizon.Domain.Entities;

namespace EventHorizon.Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
    }
}
