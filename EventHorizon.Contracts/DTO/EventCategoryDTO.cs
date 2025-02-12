namespace EventHorizon.Contracts.DTO
{
    public record EventCategoryDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
