using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;
using static HealthBridgeAPI.Services.Implementations.ModMedPatientService;

namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IModMedPatientService
    {
        Task<string> GetAllPatientsAsync();
        Task<string> GetPatientByIdAsync(string id);
        //Task<string> GetPatientByEmailAndBirthDateAsync(string email, DateTime birthDate);
        Task<PatientIfExistResponseDto> FindPatientByEmailAndBirthDateAsync(string email, DateTime birthDate, string phone);
        Task<PatientCreatedDto> RegisterPatientAsync(Patient patient);
    }
}
