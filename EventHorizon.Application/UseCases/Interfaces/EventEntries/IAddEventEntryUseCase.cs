using EventHorizon.Contracts.Responses.EventEntries;

namespace EventHorizon.Application.UseCases.Interfaces.EventEntries
{
    public interface IAddEventEntryUseCase
    {
        Task ExecuteAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
    }
}
