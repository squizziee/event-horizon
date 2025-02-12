namespace EventHorizon.Contracts.Requests.Events
{
    public record GetAllEventsRequest
    {
        public required int PageNumber;
    }
}
