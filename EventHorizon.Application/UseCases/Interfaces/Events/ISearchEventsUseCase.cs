using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Contracts.Responses.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface ISearchEventsUseCase
    {
        Task<GetEventsResponse> ExecuteAsync(SearchEventsRequest request, CancellationToken cancellationToken);
    }
}
