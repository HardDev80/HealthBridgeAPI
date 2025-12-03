using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;

namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IModMedPractitionerService
    {
        Task<string> GetAllPractitionerList();
        Task<List<Practitioner>> GetAllPractitionersPaginatedAsync();
        Task SyncPractitionersToDatabaseAsync();
        Task<List<DoctorWithSpecialityDto>> GetAllDoctorsWithSpecialityAsync();
    }
}
