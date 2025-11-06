using HealthBridgeAPI.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HealthBridgeAPI.Services
{
    public class ModMedService
    {
        private readonly HttpClient _httpClient;
        private readonly ModMedSettings _settings;

        public ModMedService(HttpClient httpClient, IOptions<ModMedSettings> settings)
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
                await RefreshAccessTokenAsync(); // <- aquí se usa
                SetAuthorizationHeader(_settings.AccessToken);
                response = await _httpClient.GetAsync(endpoint);
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Método genérico para solicitudes POST
        public async Task<string> PostAsync(string endpoint, object data)
        {
            SetAuthorizationHeader(_settings.AccessToken);

            var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RefreshAccessTokenAsync(); // <- aquí también se usa
                SetAuthorizationHeader(_settings.AccessToken);
                response = await _httpClient.PostAsync(endpoint, content);
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // 👇 MÉTODO PRIVADO (asegúrate de que esté DENTRO de la clase ModMedService)
        private async Task RefreshAccessTokenAsync()
        {
            var payload = new
            {
                grant_type = "refresh_token",
                refresh_token = _settings.RefreshToken,
                client_id = _settings.ClientId,
                client_secret = _settings.ClientSecret
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_settings.TokenEndpoint, content);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al refrescar token: {response.ReasonPhrase}");

            var result = await response.Content.ReadAsStringAsync();
            var tokenData = JsonSerializer.Deserialize<TokenResponse>(result);

            if (tokenData != null)
            {
                _settings.AccessToken = tokenData.AccessToken;
                _settings.RefreshToken = tokenData.RefreshToken ?? _settings.RefreshToken;
            }
        }

        // Clase auxiliar para deserializar la respuesta del token
        private class TokenResponse
        {
            public string AccessToken { get; set; } = string.Empty;
            public string? RefreshToken { get; set; }
        }
    }
}
