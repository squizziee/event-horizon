namespace EventHorizon.Contracts.DTO
{
    public record EventDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Address { get; set; }
        public required DateTime DateTime { get; set; }
        public required int CurrentParticipantCount { get; set; }
        public required int MaxParticipantCount { get; set; }
        public required IList<string> ImageUrls { get; set; }
        public required EventCategoryDTO Category { get; set; }
    }
}
