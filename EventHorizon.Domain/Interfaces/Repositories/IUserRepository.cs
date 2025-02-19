using EventHorizon.Domain.Entities;

namespace EventHorizon.Domain.Interfaces.Repositories
{

    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken);
        Task<User?> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken);
    }
}
