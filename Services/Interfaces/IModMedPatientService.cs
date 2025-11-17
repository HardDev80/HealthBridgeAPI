using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;

namespace HealthBridgeAPI.Services.Interfaces
{
    public interface IModMedPatientService
    {
        Task<string> GetAllPatientsAsync();
        Task<string> GetPatientByIdAsync(string id);
        //Task<string> GetPatientByEmailAndBirthDateAsync(string email, DateTime birthDate);
        Task<PatientIfExistResponseDto> FindPatientByEmailAndBirthDateAsync(string email, DateTime birthDate);
        Task<Patient> RegisterPatientAsync(Patient patient);
    }
}
