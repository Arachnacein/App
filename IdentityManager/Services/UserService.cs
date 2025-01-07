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
    }
    public interface IUserService
    {
        Task<bool> UpdateUserPropertiesAsync(UserModel model);
    }
}