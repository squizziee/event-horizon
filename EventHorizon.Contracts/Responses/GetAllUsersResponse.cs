using EventHorizon.Contracts.DTO;

namespace EventHorizon.Contracts.Responses
{
    public record GetAllUsersResponse
    {
        public required IEnumerable<UserDTO> Users { get; set; }
    }
}
