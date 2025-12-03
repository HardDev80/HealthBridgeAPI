using HealthBridgeAPI.Models.Entities;

namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IAvailableDoctorsService
    {
        Task<List<AvailableDoctor>> GetAvailableDoctorsAsync();
    }
}
