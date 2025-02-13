using EventHorizon.Domain.Enums;

namespace EventHorizon.Domain.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string PasswordHash { get; set; } = string.Empty;
		public DateOnly DateOfBirth { get; set; }
		public IList<EventEntry> Entries { get; set; } = [];
		public UserRole Role { get; set; }
		public string RefreshToken { get; set; } = string.Empty;
	}
}
