using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Interfaces;

namespace HealthBridgeAPI.Services.Implementations
{
    public class ModMedPractitionerService : IModMedPractitionerService

    {
        private readonly IModMedRepository _modMedRepository;
        private readonly IConfiguration _configuration;

        public ModMedPractitionerService(IModMedRepository modMedRepository, IConfiguration configuration)
        {
            _modMedRepository = modMedRepository;
            _configuration = configuration;
        }

        public async Task<string> GetAllPractitionerList()
        {
            var endpoint = "Practitioner";
            var token = _configuration["ModMedSettings:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }

    }
}
