namespace EventHorizon.Application.UseCases.Interfaces.Users
{
    public interface IRefreshTokensUseCase
    {
        Task<(string, string)> ExecuteAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
