using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.Events
{
    public record GetEventsResponse
    {
        public required IEnumerable<EventDTO> Events { get; set; }
        public required int PageNumber { get; set; }
        public required int TotalPages { get; set; }
    }
}
