namespace EventHorizon.Contracts.Requests
{
    public record RegsiterUserRequest
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly DateOfBirth { get; set; }
    }
}
