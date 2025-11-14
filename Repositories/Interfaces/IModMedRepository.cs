using System.Threading.Tasks;

namespace HealthBridgeAPI.Repositories.Interfaces
{
    public interface IModMedRepository
    {
        Task<string> GetAsync(string endpoint, string accessToken);
        Task<string> GetPatientByEmailAndBirthDateAsync(string PatientEmail, string PatientBirthDate, string accessToken);
    }
}
