namespace EventHorizon.Contracts.DTO
{
    public record UserDTO
    {
        public required Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required DateOnly DateOfBirth { get; set; }
    }
}
