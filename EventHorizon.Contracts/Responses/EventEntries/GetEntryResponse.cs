using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses.EventEntries
{
    public record GetEntryResponse
    {
        public required EventEntryDTO Entry { get; set; }
    }
}
