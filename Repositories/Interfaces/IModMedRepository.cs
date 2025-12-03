using System.Threading.Tasks;

namespace HealthBridgeAPI.Repositories.Interfaces
{
    public interface IModMedRepository
    {
        Task<string> GetAsync(string endpoint, string accessToken);
        Task<string> GetPatientByEmailAndBirthDateAsync(string PatientEmail, string PatientBirthDate, string accessToken);
        Task<string> PostAsync(string endpoint, string bodyJson, string accessToken = null);
        Task<HttpResponseMessage> PostRawAsync(string endpoint, string json, string accessToken = null);

    }
}
