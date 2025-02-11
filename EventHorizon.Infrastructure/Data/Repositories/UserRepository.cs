using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Infrastructure.Data.Repositories.Interfaces;

namespace EventHorizon.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User entity, CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public Task DeleteAsync(User entity, CancellationToken cancellationToken)
        {
            _context.Users.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<User?> FindByEmail(string email)
        {
            return Task.FromResult(
                _context.Users.FirstOrDefault(u => u.Email == email)
            );
        }

        public Task<User?> FindByRefreshToken(string token)
        {
            return Task.FromResult(
                _context.Users.FirstOrDefault(u => u.RefreshToken == token)
            );
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Users.AsEnumerable()
            );
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Users.FirstOrDefault(u => u.Id == id)
            );
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            var tryFind = _context.Users.FirstOrDefault(u => u.Id == entity.Id);

            if (tryFind == null)
            {
                throw new ResourceNotFoundException($"No user with id {entity.Id} was found");
            }

            tryFind.FirstName = entity.FirstName;
            tryFind.LastName = entity.LastName;
            tryFind.DateOfBirth = entity.DateOfBirth;

            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
