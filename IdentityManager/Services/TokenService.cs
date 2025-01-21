using IdentityManager.Exceptions;
using IdentityManager.Models;
using IdentityManager.Models.Enums;
using System.Net.Http.Headers;
using System.Text.Json;

namespace IdentityManager.Services
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAdminTokenAsync()
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
    }
    public interface ITokenService
    { 
        Task<string> GetAdminTokenAsync();
    }
}