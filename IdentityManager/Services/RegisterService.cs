using IdentityManager.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IdentityManager.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient _httpClient;

        public RegisterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Register(RegistrationModel model)
        {
            var adminToken = await GetAdminToken();
            if (string.IsNullOrEmpty(adminToken))
                return false;

            var requestBody = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                username = model.Username,
                email = model.Email,
                enabled = true,
                credentials = new[] {new { type = "password", value = model.Password, temporary = false }}
            };
            var jsonContent = new StringContent(
                                     JsonSerializer.Serialize(requestBody),
                                     System.Text.Encoding.UTF8,
                                     "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "http://keycloak:8080/admin/realms/AppRealm/users")
            {
                Content = jsonContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        public async Task<string> GetAdminToken()
        {
            var tokenRequest = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "admin-cli"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "admin"),
                new KeyValuePair<string, string>("password", "admin")
            });

            var response = await _httpClient
                .PostAsync("http://keycloak:8080/realms/master/protocol/openid-connect/token", tokenRequest);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to get admin token. Response: {err}");
                return null;
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseData);
            return json.RootElement.GetProperty("access_token").GetString();
        }
    }
    public interface IRegisterService
    {
        Task<bool> Register(RegistrationModel model);
        Task<string> GetAdminToken();
    }
}