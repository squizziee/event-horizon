namespace EventHorizon.Domain.Entities
{
    public class EventCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IList<Event> Events { get; set; } = [];
    }
}
