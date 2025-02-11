namespace EventHorizon.Infrastructure.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string providedPassword, string validPasswordHash);
    }
}
