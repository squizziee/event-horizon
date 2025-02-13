namespace EventHorizon.Application.UseCases.Interfaces.EventEntries
{
    public interface IDeleteEventEntryUseCase
    {
        Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
