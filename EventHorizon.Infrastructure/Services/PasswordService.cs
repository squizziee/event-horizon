using EventHorizon.Infrastructure.Services.Interfaces;

namespace EventHorizon.Infrastructure.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

            return hash;
        }

        public bool VerifyPassword(string providedPassword, string validPasswordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(providedPassword, validPasswordHash);
        }
    }
}
