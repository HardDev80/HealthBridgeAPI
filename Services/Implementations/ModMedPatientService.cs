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

        public async Task<PatientIfExistResponseDto?> FindPatientByEmailAndBirthDateAsync(string email, DateTime birthDate, string phone)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var token = _configuration["ModMedSettings:AccessToken"];

            var birth = birthDate.ToString("yyyy-MM-dd");
            var endpoint = $"Patient?birthdate={Uri.EscapeDataString(birth)}&email={Uri.EscapeDataString(email)}&phone={Uri.EscapeDataString(phone)}";

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

        public class PatientCreatedDto
        {
            public string? PatientFirstName { get; set; }
            public string? PatientLastName { get; set; }
            public string? PatientEmail { get; set; }
            public DateTime? PatientBirthDate { get; set; }
            public string? PatientPMS { get; set; }
            public string? PatientPhone { get; set; }
        }

        public async Task<PatientCreatedDto> RegisterPatientAsync(Patient patient)
        {
            try
            {
                var token = _configuration["ModMedSettings:AccessToken"];

                // Construcción del payload FHIR (igual que antes)
                var payload = new
                {
                    resourceType = "Patient",
                    name = new[]
                    {
                new {
                    given = new[] { patient.PatientFirstName },
                    family = patient.PatientLastName
                }
            },
                    gender = patient.PatientGender?.ToLower(),
                    birthDate = patient.PatientBirthDate,
                    telecom = new[]
                    {
                new { system = "email", value = patient.PatientEmail },
                new { system = "phone", value = patient.PatientMobilePhone },
                new { system = "phone", value = patient.PatientHomePhone }
            },
                    address = new[]
                    {
                new {
                    line = new[] { patient.PatientAddress },
                    city = patient.PatientCity,
                    state = patient.PatientState,
                    postalCode = patient.PatientPostalCode,
                    country = patient.PatientCountry
                }
            },
                    maritalStatus = new { text = patient.PatientMaritalStatus },
                    identifier = new[]
                    {
                new { system = "PMS", value = patient.PatientPMS },
                new { system = "http://hl7.org/fhir/sid/us-ssn", value = patient.PatientSSN }
            }
                };

                string jsonBody = JsonSerializer.Serialize(payload);

                // 🔥 Llamamos la nueva función
                var response = await _modMedRepository.PostRawAsync("Patient", jsonBody, token);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception(
                        $"ModMed error {response.StatusCode}: {error}"
                    );
                }

                // 🍀 Registro exitoso aun si no hay body
                return new PatientCreatedDto
                {
                    PatientFirstName = patient.PatientFirstName,
                    PatientLastName = patient.PatientLastName,
                    PatientEmail = patient.PatientEmail,
                    PatientBirthDate = patient.PatientBirthDate,
                    PatientPhone = patient.PatientMobilePhone,
                    PatientPMS = patient.PatientPMS  // no podemos obtener el nuevo ID FHIR porque el sandbox no lo devuelve
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error registrando paciente en ModMed: {ex.Message}", ex);
            }
        }
 







    }
}
    

