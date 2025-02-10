namespace EventHorizon.Domain.Entities
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime DateTime { get; set; }
        public int MaxParticipantCount { get; set; }
        public Guid CategoryId { get; set; }
        public EventCategory Category { get; set; }
        public IEnumerable<EventEntry> Entries { get; set; } = [];
    }
}
