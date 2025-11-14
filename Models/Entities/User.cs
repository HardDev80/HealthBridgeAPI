using System.ComponentModel.DataAnnotations;

namespace HealthBridgeAPI.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = null;
        [Required]
        public string? UserEmail { get; set; }
        [Required]
        public string UserPasswordHash { get; set; } = null;
        public string UserPasswordSalt { get; set; } = null;
        [Required]
        public string? UserRole { get; set; }
        public bool UserIsActive { get; set; } = true;
        public DateTime UserCreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UserLastLoginAt { get; set; }
        public int UserFailedAttempts { get; set; } = 0;
        public DateTime? UserLockoutUntil { get; set; }
        public int? UserOrganizationId { get; set; }
    }
}
