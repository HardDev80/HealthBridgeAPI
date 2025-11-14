using HealthBridgeAPI.Services.Interfaces;
using HealthBridgeAPI.Repositories.Interfaces;

namespace HealthBridgeAPI.Services.Implementations
{
    public class ModMedAppointmentService : IModMedAppointmentService
    {

        private readonly IModMedRepository _modMedRepository;
        private readonly IConfiguration _configuration;

        public ModMedAppointmentService(IModMedRepository modMedRepository, IConfiguration configuration)
        {
            _modMedRepository = modMedRepository;
            _configuration = configuration;
        }

        public async Task<string> GetAllAppointmentList()
        {
            var endpoint = "Appointment";
            var token = _configuration["ModMedSettings:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint,token);
        }

        
    }
}
