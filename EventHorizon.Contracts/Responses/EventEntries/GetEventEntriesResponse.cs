using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.EventEntries
{
    public class GetEventEntriesResponse
    {
        public required IEnumerable<EventEntryDTO> Entries { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
    }
}
