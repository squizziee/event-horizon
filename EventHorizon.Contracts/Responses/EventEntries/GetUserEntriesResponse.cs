using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.EventEntries
{
    public record GetUserEntriesResponse
    {
        public required IEnumerable<UserEventEntryDTO> Entries { get; set; }
    }
}
