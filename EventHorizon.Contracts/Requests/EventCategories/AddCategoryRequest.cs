namespace EventHorizon.Contracts.Requests.EventCategories
{
    public record AddCategoryRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
