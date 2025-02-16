using Microsoft.AspNetCore.Http;

namespace EventHorizon.Contracts.Requests.Events
{
    public record AddEventRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeOnly Time { get; set; }
        public required int MaxParticipantCount { get; set; }
        public required Guid CategoryId { get; set; }
        public IFormFileCollection? AttachedImages { get; set; }
    }
}
