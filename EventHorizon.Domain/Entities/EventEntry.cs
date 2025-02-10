namespace EventHorizon.Domain.Entities
{
    public class EventEntry
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
