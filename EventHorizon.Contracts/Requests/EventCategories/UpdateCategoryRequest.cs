namespace EventHorizon.Contracts.Requests.EventCategories
{
    public record UpdateCategoryRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
