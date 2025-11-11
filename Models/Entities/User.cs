namespace HealthBridgeAPI.Models.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null;
        public string? UserEmail { get; set; }
        public string UserPasswordHash { get; set; } = null;
        public string UserPasswordSalt { get; set; } = null;
        public string? UserRole { get; set; }
        public Boolean UserIsActive { get; set; } = true;
        public DateTime UserCreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UserLastLoginAt { get; set; }
        public int UserFailedAttempts { get; set; } = 0;
        public DateTime? UserLockoutUntil { get; set; }
        public int? UserOrganizationId { get; set; }
    }
}
