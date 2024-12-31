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
        public async Task<bool> UpdateUserProperties(UserModel model)
        {
            //check token
            var adminToken = await _tokenService.GetAdminTokenAsync();
            var serializedContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
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
        Task<bool> UpdateUserProperties(UserModel model);
    }
}