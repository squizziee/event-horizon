using EventHorizon.Contracts.Requests.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface IUpdateEventUseCase
    {
        Task ExecuteAsync(Guid id, UpdateEventRequest request, CancellationToken cancellationToken);
    }
}
