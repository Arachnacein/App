using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace IdentityManager.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;
        public RegisterService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        public async Task<bool> RegisterAsync(RegistrationModel model)
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                return false;

            if (await UsernameExistsAsync(model.Username))
                throw new CustomException((int)ErrorCodesEnum.UsernameAlreadyExists, "Username already exists.");

            if (await EmailExistsAsync(model.Email))
                throw new CustomException((int)ErrorCodesEnum.EmailAlreadyExists, "Email already exists.");

            var requestBody = new
            {
                firstName = model.FirstName,
                lastName = model.LastName,
                username = model.Username,
                email = model.Email,
                enabled = true,
                credentials = new[] {new { type = "password", value = model.Password, temporary = false }}
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "http://keycloak:8080/admin/realms/AppRealm/users")
            {
                Content = jsonContent
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                return false;

            var request = new HttpRequestMessage(HttpMethod.Get, 
                                        $"http://keycloak:8080/admin/realms/AppRealm/users?username={username}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User with username '{username}' not found.");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<object>>(responseData);

            return users?.Any() ?? false; // if exists -> true
        }        
        public async Task<bool> EmailExistsAsync(string email)
        {
            var adminToken = await _tokenService.GetAdminTokenAsync();
            if (string.IsNullOrEmpty(adminToken))
                return false;
            var request = new HttpRequestMessage(HttpMethod.Get,
                            $"http://keycloak:8080/admin/realms/AppRealm/users?email={email}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new CustomException((int)ErrorCodesEnum.UserNotFound, $"User with email '{email}'not found.");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<object>>(responseData); 

            return users?.Any() ?? false; // if exists -> true
        }
    }
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegistrationModel model);
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}