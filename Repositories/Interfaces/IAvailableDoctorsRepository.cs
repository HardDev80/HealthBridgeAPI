using HealthBridgeAPI.Models.Entities;

namespace HealthBridgeAPI.Repositories.Interfaces
{
    public interface IAvailableDoctorsRepository
    {
        Task<List<AvailableDoctor>> GetAvailableDoctorsAsync();
    }

}
