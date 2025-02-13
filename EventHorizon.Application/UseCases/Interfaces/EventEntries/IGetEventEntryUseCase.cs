using EventHorizon.Contracts.Responses.EventEntries;

namespace EventHorizon.Application.UseCases.Interfaces.EventEntries
{
    public interface IGetEventEntryUseCase
    {
        Task<GetEntryResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
