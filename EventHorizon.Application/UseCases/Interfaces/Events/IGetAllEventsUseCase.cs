using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Contracts.Responses.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface IGetAllEventsUseCase
    {
        Task<GetEventsResponse> ExecuteAsync(GetAllEventsRequest request, CancellationToken cancellationToken);
    }
}
