namespace EventHorizon.Contracts.DTO
{
    public record EventEntryDTO
    {
        public required Guid Id { get; set; }
        public required UserDTO User { get; set; }
        public required DateTime SubmissionDate { get; set; }
    }
}
