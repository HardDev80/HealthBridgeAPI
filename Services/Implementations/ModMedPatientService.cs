using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace HealthBridgeAPI.Services.Implementations
{
    public class ModMedPatientService : IModMedPatientService
    {
        private readonly IModMedRepository _modMedRepository;
        private readonly IConfiguration _configuration;

        public ModMedPatientService(IModMedRepository modMedRepository, IConfiguration configuration)
        {
            _modMedRepository = modMedRepository;
            _configuration = configuration;
        }

        public async Task<string> GetAllPatientsAsync()
        {
            var endpoint = "Patient";  // en la base FHIR de ModMed, este es el recurso principal
            var token = _configuration["ModMedSettings:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }

        public async Task<string> GetPatientByIdAsync(string patientId)
        {
            var endpoint = $"Patient/{patientId}";
            var token = _configuration["ModMed:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }
    }
}
