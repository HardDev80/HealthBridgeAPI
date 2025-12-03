using HealthBridgeAPI.Models;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Interfaces;

namespace HealthBridgeAPI.Services
{
    public class AvailableDoctorsService : IAvailableDoctorsService
    {
        private readonly IAvailableDoctorsRepository _repository;

        public AvailableDoctorsService(IAvailableDoctorsRepository repository)
        {
            _repository = repository;
        }

        public Task<List<AvailableDoctor>> GetAvailableDoctorsAsync()
        {
            return _repository.GetAvailableDoctorsAsync();
        }
    }
}
