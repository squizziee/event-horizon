namespace EventHorizon.Contracts.Requests.EventEntries
{
    public record GetEventEntriesRequest
    {
        public required int PageNumber { get; set; }
    }
}
