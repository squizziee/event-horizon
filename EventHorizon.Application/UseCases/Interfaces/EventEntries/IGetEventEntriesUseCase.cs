using EventHorizon.Contracts.Requests.EventEntries;
using EventHorizon.Contracts.Responses.EventEntries;

namespace EventHorizon.Application.UseCases.Interfaces.EventEntries
{
    public interface IGetEventEntriesUseCase
    {
        Task<GetEventEntriesResponse> ExecuteAsync(Guid eventId, GetEventEntriesRequest request, CancellationToken cancellationToken);
    }
}
