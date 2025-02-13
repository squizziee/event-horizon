using EventHorizon.Contracts.Responses.EventEntries;

namespace EventHorizon.Application.UseCases.Interfaces.EventEntries
{
    public interface IGetUserEntriesUseCase
    {
        Task<GetUserEntriesResponse> ExecuteAsync(Guid userid, CancellationToken cancellationToken);
    }
}
