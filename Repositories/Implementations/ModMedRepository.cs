using HealthBridgeAPI.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HealthBridgeAPI.Repositories.Implementations
{
    public class ModMedRepository : IModMedRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _accessToken;

        public ModMedRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // 1️⃣ Base URL
            var baseUrl = _configuration["ModMedSettings:BaseUrl"];
            if (!string.IsNullOrEmpty(baseUrl))
            {
                if (!baseUrl.EndsWith("/"))
                    baseUrl += "/";
                _httpClient.BaseAddress = new Uri(baseUrl);
            }

            // 2️⃣ Headers globales
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            var apiKey = _configuration["ModMedSettings:ApiKey"];
            if (!string.IsNullOrEmpty(apiKey) && !_httpClient.DefaultRequestHeaders.Contains("x-api-key"))
                _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);

            // 3️⃣ Token fijo desde configuración
            _accessToken = _configuration["ModMedSettings:AccessToken"];
        }

        public async Task<string> GetAsync(string endpoint, string accessToken = null)
        {
            // 4️⃣ Usar token de parámetro o el fijo del appsettings
            var tokenToUse = !string.IsNullOrWhiteSpace(accessToken)
                ? accessToken
                : _accessToken;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenToUse);

            // 5️⃣ Construir URI válida
            Uri requestUri = endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? new Uri(endpoint)
                : new Uri(_httpClient.BaseAddress!, endpoint.TrimStart('/'));

            // 6️⃣ Hacer GET
            var response = await _httpClient.GetAsync(requestUri);
            var content = await response.Content.ReadAsStringAsync();

            // 7️⃣ Manejar errores
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"ModMed GET {requestUri} returned {(int)response.StatusCode} {response.ReasonPhrase}: {content}"
                );
            }

            return content;
        }


        public async Task<string> PostAsync(string endpoint, string bodyJson, string accessToken = null)
        {
            // 1️⃣ Escoger token: el que viene o el fijo del appsettings
            var tokenToUse = !string.IsNullOrWhiteSpace(accessToken)
                ? accessToken
                : _accessToken;

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", tokenToUse);

            // 2️⃣ Construir URI válida (igual que en GetAsync)
            Uri requestUri = endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                ? new Uri(endpoint)
                : new Uri(_httpClient.BaseAddress!, endpoint.TrimStart('/'));

            // 3️⃣ Contenido del body
            var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            // si ModMed exige "application/fhir+json", aquí lo cambias

            // 4️⃣ Hacer POST
            var response = await _httpClient.PostAsync(requestUri, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // 5️⃣ Manejar errores
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(
                    $"ModMed POST {requestUri} returned {(int)response.StatusCode} {response.ReasonPhrase}: {responseContent}"
                );
            }

            return responseContent;
        }

        public async Task<string> GetPatientByEmailAndBirthDateAsync(string PatientEmail, string PatientBirthDate, string accessToken)
        {
            var endpoint = $"Patient?email={PatientEmail}&birthdate={PatientBirthDate}";
            return await GetAsync(endpoint,accessToken);
        }

        
    }
}
