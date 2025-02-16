namespace EventHorizon.Application.UseCases.Interfaces.Dev
{
    public interface ICreateAdminUseCase
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
