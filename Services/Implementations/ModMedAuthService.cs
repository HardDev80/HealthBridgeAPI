using HealthBridgeAPI.Services.Interfaces;
using HealthBridgeAPI.Utils;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HealthBridgeAPI.Services.Implementations
{
    public class ModMedAuthService : IModMedAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ModMedSettings _settings;

        public ModMedAuthService(HttpClient httpClient, IOptions<ModMedSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _settings.ApiKey);
        }

        // Método para añadir el token al header
        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        // Método genérico para solicitudes GET
        public async Task<string> GetAsync(string endpoint)
        {
            SetAuthorizationHeader(_settings.AccessToken);

            var response = await _httpClient.GetAsync(endpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // En modo sandbox no se usa refresh token, solo mostramos advertencia
                Console.WriteLine("⚠️ Token no autorizado. Verifica tu AccessToken o Sandbox actual.");
                throw new Exception("El token actual no es válido en este entorno.");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }



        // Método genérico para solicitudes POST (opcional)
        /*
        public async Task<string> PostAsync(string endpoint, object data)
        {
            SetAuthorizationHeader(_settings.AccessToken);

            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine("⚠️ Token no autorizado. Verifica tu AccessToken o Sandbox actual.");
                throw new Exception("El token actual no es válido en este entorno.");
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync()
        }
        */
    }
}
