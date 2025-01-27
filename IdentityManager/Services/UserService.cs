using IdentityManager.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IdentityManager.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public UserService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<bool> UpdateUserPropertiesAsync(UserModel model)
        {
            //check token
            var adminToken = await _tokenService.GetAdminTokenAsync();
            var payload = new
            {
                username = model.Username,
                email = model.Email,
                firstName = model.FirstName,
                lastName = model.LastName,
                enabled = true
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var serializedContent = new StringContent(JsonSerializer.Serialize(payload, options), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Put, $"http://keycloak:8080/admin/realms/AppRealm/users/{model.UserId}")
            {
                Content = serializedContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);
            return response.IsSuccessStatusCode;
        }

        public async Task<UserModel> GetUserDataAsync(Guid userId)
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                throw new Exception("Failed to retrieve admin token.");

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://keycloak:8080/admin/realms/AppRealm/users/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserModel>(responseData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (user == null)
                throw new Exception($"User not found. ID: {userId}");

            return user;
        }

        public async Task SendVerificationEmailAsync(Guid userId)
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                throw new Exception("Failed to retrieve admin token.");

            var request = new HttpRequestMessage(HttpMethod.Put,
                                                 $"http://keycloak:8080/admin/realms/AppRealm/users/{userId}/send-verify-email");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Error while sending email verification request to keycloak.");
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                throw new Exception("Failed to retrieve admin token.");

            var request = new HttpRequestMessage(HttpMethod.Get, "http://keycloak:8080/admin/realms/AppRealm/users");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserModel>>(responseData, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (users == null)
                throw new Exception("Failed to retrieve users.");

            return users;
        }
    }
    public interface IUserService
    {
        Task<bool> UpdateUserPropertiesAsync(UserModel model);
        Task<UserModel> GetUserDataAsync(Guid userId);
        Task SendVerificationEmailAsync(Guid userId);
        Task<List<UserModel>> GetUsersAsync();
    }
}