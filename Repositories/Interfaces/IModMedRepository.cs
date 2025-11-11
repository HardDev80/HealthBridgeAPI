using System.Threading.Tasks;

namespace HealthBridgeAPI.Repositories.Interfaces
{
    public interface IModMedRepository
    {
        Task<string> GetAsync(string endpoint, string accessToken);
    }
}
