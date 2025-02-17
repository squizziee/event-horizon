using EventHorizon.Domain.Entities;

namespace EventHorizon.Domain.Interfaces.Repositories
{

    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByEmail(string email);
        Task<User?> FindByRefreshToken(string token);
    }
}
