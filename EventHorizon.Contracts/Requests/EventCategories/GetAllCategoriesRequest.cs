namespace EventHorizon.Contracts.Requests.EventCategories
{
    public record GetAllCategoriesRequest
    {
        public required int PageNumber { get; set; }
        public required bool DoNotPaginate { get; set; } = false;
}
}
