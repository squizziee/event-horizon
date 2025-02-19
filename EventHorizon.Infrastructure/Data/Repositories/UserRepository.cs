using EventHorizon.Contracts.Exceptions;
using EventHorizon.Domain.Entities;
using EventHorizon.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var result = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            return result;
        }

        public async Task<User?> FindByRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var result = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == token, cancellationToken);

            return result;
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(
                _context.Users.AsEnumerable()
            );
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            return result;
        }

        public async Task UpdateAsync(User entity, CancellationToken cancellationToken)
        {
            var tryFind = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

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
