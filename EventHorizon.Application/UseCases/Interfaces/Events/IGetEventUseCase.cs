using EventHorizon.Contracts.Responses.Events;

namespace EventHorizon.Application.UseCases.Interfaces.Events
{
    public interface IGetEventUseCase
    {
        Task<GetOneEventResponse> ExecuteAsync(Guid id, CancellationToken cancellationToken);
    }
}
