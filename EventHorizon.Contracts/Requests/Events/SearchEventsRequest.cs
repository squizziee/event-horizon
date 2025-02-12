namespace EventHorizon.Contracts.Requests.Events
{
    public record SearchEventsRequest
    {
        public string? TextQuery { get; set; }
        public string? PlaceQuery { get; set; }
        public IEnumerable<Guid>? Categories { get; set; }
        public DateOnly? SearchFromDate { get; set; }
        public DateOnly? SearchUntilDate { get; set; }
        public required int PageNumber { get; set; }
    }
}
