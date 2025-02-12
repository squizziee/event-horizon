namespace EventHorizon.Application.Helpers
{
    public record PaginationOptions
    {
        public required int PageSize { get; set; }
    }
}
