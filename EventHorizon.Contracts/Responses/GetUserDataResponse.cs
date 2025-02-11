using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses
{
    public record GetUserDataResponse
    {
        public required UserDTO User { get; set; }
    }
}
