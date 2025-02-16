namespace EventHorizon.Application.UseCases.Interfaces.Dev
{
    public interface ISeedDatabaseUseCase
    {
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
