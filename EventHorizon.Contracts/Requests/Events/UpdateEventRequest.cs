using Microsoft.AspNetCore.Http;

namespace EventHorizon.Contracts.Requests.Events
{
    public record UpdateEventRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required DateTime DateTime { get; set; }
        public required int MaxParticipantCount { get; set; }
        public required Guid CategoryId { get; set; }
        public required IFormFileCollection AttachedImages { get; set; }
    }
}
