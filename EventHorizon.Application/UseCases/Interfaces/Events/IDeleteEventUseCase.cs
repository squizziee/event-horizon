using EventHorizon.Contracts.Requests.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface IDeleteEventUseCase
    {
        Task ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
