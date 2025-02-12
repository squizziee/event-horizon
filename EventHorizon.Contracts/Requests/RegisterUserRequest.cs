namespace EventHorizon.Contracts.Requests
{
    public record RegisterUserRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
    }
}
