namespace EventHorizon.Contracts.DTO
{
    public record UserEventEntryDTO
    {
        public required Guid Id { get; set; }
        public required EventDTO Event { get; set; }
        public required DateTime SubmissionDate { get; set; }
    }
}
