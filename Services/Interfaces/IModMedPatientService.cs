namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IModMedPatientService
    {
        Task<string> GetAllPatientsAsync();
        Task<string> GetPatientByIdAsync(string patientId);
    }
}
