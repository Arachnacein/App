using Flurl.Http;
using IdentityManager.Models;
using Keycloak.Net;
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

            Console.WriteLine($"Admin token: {adminToken}");

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://keycloak:8080/admin/realms/AppRealm/users/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            Console.WriteLine("Before request");

            try
            {
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseData = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserModel>(responseData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                Console.WriteLine("After request");

                if (user == null)
                    throw new Exception($"User not found. ID: {userId}");

                return user;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
                throw;
            }
            //var adminToken = await _tokenService.GetAdminTokenAsync();
            //Console.WriteLine($"_____________ admin token {adminToken}");
            //var _keycloak = new KeycloakClient("http://keycloak:8080", adminToken);

            //Console.WriteLine("Przed zpaytaniem");
            //Console.WriteLine(userId.ToString());
            //var user = await _keycloak.GetUserAsync("AppRealm", userId.ToString());
            //Console.WriteLine("_____________ Po zapytaniu");

            //if(user == null)
            //    throw new Exception($"User is null. Id: {userId}");
            //return new UserModel
            //{
            //    FirstName = user.FirstName,
            //    LastName = user.LastName,
            //    Email = user.Email,
            //    Username = user.UserName
            //};
        }
    }
    public interface IUserService
    {
        Task<bool> UpdateUserPropertiesAsync(UserModel model);
        Task<UserModel> GetUserDataAsync(Guid userId);
    }
}