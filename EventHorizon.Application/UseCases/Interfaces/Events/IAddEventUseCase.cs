using EventHorizon.Contracts.Requests.Events;
using EventHorizon.Contracts.Responses.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface IAddEventUseCase
    {
        Task ExecuteAsync(AddEventRequest request, CancellationToken cancellationToken);
    }
}
