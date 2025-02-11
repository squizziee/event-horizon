using EventHorizon.Domain.Entities;

namespace EventHorizon.Infrastructure.Data.Repositories.Interfaces
{

    public interface IUserRepository : IRepository<User>
    {
        Task<User?> FindByEmail(string email);
        Task<User?> FindByRefreshToken(string token);
    }
}
