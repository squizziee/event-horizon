namespace EventHorizon.Infrastructure.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyHash(string providedPassword, string validPasswordHash);
    }
}
