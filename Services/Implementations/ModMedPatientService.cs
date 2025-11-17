using HealthBridgeAPI.Data;
using HealthBridgeAPI.Models.DTOs;
using HealthBridgeAPI.Models.Entities;
using HealthBridgeAPI.Repositories.Interfaces;
using HealthBridgeAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;

namespace HealthBridgeAPI.Services.Implementations
{
    public class ModMedPatientService : IModMedPatientService
    {
        private readonly IModMedRepository _modMedRepository;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;

        public ModMedPatientService(IModMedRepository modMedRepository, IConfiguration configuration, AppDbContext context, HttpClient http)
        {
            _modMedRepository = modMedRepository;
            _configuration = configuration;
            _context = context;
            _httpClient = http;
        }

        public async Task<string> GetAllPatientsAsync()
        {
            var endpoint = "Patient";  // en la base FHIR de ModMed, este es el recurso principal
            var token = _configuration["ModMedSettings:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }

        public async Task<string> GetPatientByIdAsync(string id)
        {
            var endpoint = $"Patient/{id}";
            var token = _configuration["ModMed:AccessToken"];
            return await _modMedRepository.GetAsync(endpoint, token);
        }

        /* public async Task<string> GetPatientByEmailAndBirthDateAsync()
          {
              var endpoint = "Patient?birthdate=1972-02-22&email=hoakes@nowhere.com";
              var token = _configuration["ModMed:AccessToken"];
              return await _modMedRepository.GetAsync(endpoint,token);
          }*/

        public async Task<PatientIfExistResponseDto?> FindPatientByEmailAndBirthDateAsync(
    string email, DateTime birthDate)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var token = _configuration["ModMedSettings:AccessToken"];

            var birth = birthDate.ToString("yyyy-MM-dd");
            var endpoint = $"Patient?birthdate={Uri.EscapeDataString(birth)}&email={Uri.EscapeDataString(email)}";

            var json = await _modMedRepository.GetAsync(endpoint, token);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("entry", out var entries) &&
                    entries.ValueKind == JsonValueKind.Array &&
                    entries.GetArrayLength() > 0)
                {
                    var entry = entries[0];

                    if (entry.TryGetProperty("resource", out var resource))
                    {
                        var id = resource.TryGetProperty("id", out var idProp)
                            ? idProp.GetString()
                            : null;

                        string given = "";
                        string family = "";

                        // Name
                        if (resource.TryGetProperty("name", out var nameArr) &&
                            nameArr.ValueKind == JsonValueKind.Array &&
                            nameArr.GetArrayLength() > 0)
                        {
                            var nameObj = nameArr[0];

                            // given (primer nombre)
                            if (nameObj.TryGetProperty("given", out var givenArr) &&
                                givenArr.ValueKind == JsonValueKind.Array &&
                                givenArr.GetArrayLength() > 0)
                            {
                                given = string.Join(" ", givenArr.EnumerateArray().Select(x => x.GetString()));
                            }

                            // family (apellido)
                            if (nameObj.TryGetProperty("family", out var familyProp))
                            {
                                family = familyProp.GetString() ?? "";
                            }
                        }

                        // DTO de respuesta
                        return new PatientIfExistResponseDto
                        {
                            PatientExternalId = id,
                            PatientFirstName = given,
                            PatientLastName = family
                        };
                    }
                }

                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        public async Task<Patient> RegisterPatientAsync(Patient patient)
        {
            if (patient == null)
                throw new ValidationException("La información del paciente no puede ser nula.");

            if (patient.PatientBirthDate == null)
                throw new ValidationException("La fecha de nacimiento es obligatoria.");

            
            bool existsLocal = await _context.Patients.AnyAsync(p =>
                p.PatientEmail == patient.PatientEmail &&
                p.PatientBirthDate == patient.PatientBirthDate);

            if (existsLocal)
                throw new ValidationException("Ya existe un paciente con este email y fecha de nacimiento en la base local.");

            
            var fhirPatient = new
            {
                resourceType = "Patient",
                name = new[]
                {
            new {
                given  = new[] { patient.PatientFirstName ?? "" },
                family = patient.PatientLastName ?? ""
            }
        },
                gender = string.IsNullOrWhiteSpace(patient.PatientGender)
                    ? null
                    : patient.PatientGender.ToLower(),
                birthDate = patient.PatientBirthDate.Value.ToString("yyyy-MM-dd"),
                telecom = new[]
                {
            new {
                system = "phone",
                value  = patient.PatientMobilePhone,
                use    = "mobile"
            },
            new {
                system = "email",
                value  = patient.PatientEmail,
                use    = "home"
            }
        },
                address = string.IsNullOrWhiteSpace(patient.PatientAddress)
                    ? null
                    : new[]
                    {
                new { text = patient.PatientAddress }
                    }
            };

            var jsonBody = JsonSerializer.Serialize(
                fhirPatient,
                new JsonSerializerOptions
                {
                    DefaultIgnoreCondition =
                        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

            
            var token = _configuration["ModMedSettings:AccessToken"];
            var responseJson = await _modMedRepository.PostAsync("Patient", jsonBody, token);

            
            string? externalId = null;
            try
            {
                using var doc = JsonDocument.Parse(responseJson);
                var root = doc.RootElement;
                if (root.TryGetProperty("id", out var idProp))
                {
                    externalId = idProp.GetString();
                }
            }
            catch (JsonException ex)
            {
                throw new ValidationException(
                    $"La respuesta de ModMed no tiene el formato esperado. Detalle: {ex.Message}. Respuesta: {responseJson}"
                );
            }

            
            if (!string.IsNullOrWhiteSpace(externalId))
            {
                
                patient.PatientPMS = externalId;
            }

            if (string.IsNullOrWhiteSpace(patient.PatientStatus))
            {
                patient.PatientStatus = "Active";
            }

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return patient;
        }

    }
}
    

