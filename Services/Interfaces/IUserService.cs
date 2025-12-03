using HealthBridgeAPI.Models.Entities;

namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(string userName, string email, string clientHashedPassword /* SHA-256 hex/base64 from front */, string? role = null);
        Task<bool> ValidateLoginAsync(string userName, string clientHashedPassword);
        Task<User?> GetByUserNameAsync(string userName);
    }

}
