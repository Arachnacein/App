using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
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

            if (await UsernameExists(model.Username))
            {
                Console.WriteLine($"throwed exception username already exists. code:{(int)ErrorCodesEnum.UsernameAlreadyExists}");
                throw new CustomException((int)ErrorCodesEnum.UsernameAlreadyExists, "Username already exists.");
            }

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
                var error = await response.Content.ReadAsStringAsync();
                throw new CustomException((int)ErrorCodesEnum.AdminTokenFetchFailed, "Failed to fetch admin token.");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseData);
            return json.RootElement.GetProperty("access_token").GetString();
        }
        public async Task<bool> UsernameExists(string username)
        {
            var adminToken = await GetAdminToken();
            if (string.IsNullOrEmpty(adminToken))
                return false;

            var request = new HttpRequestMessage(HttpMethod.Get, 
                                        $"http://keycloak:8080/admin/realms/AppRealm/users?username={username}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"failed to check if user exists. response: {error}");
                return false;
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<object>>(responseData); // Deserialize to a list of users

            return users?.Any() ?? false; // if exists -> true
        }        
        public async Task<bool> EmailExists(string username)
        {
            throw new NotImplementedException();
        }
    }
    public interface IRegisterService
    {
        Task<bool> Register(RegistrationModel model);
        Task<string> GetAdminToken();
        Task<bool> UsernameExists(string username);
        Task<bool> EmailExists(string email);
    }
}