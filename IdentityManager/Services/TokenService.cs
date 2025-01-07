using IdentityManager.Exceptions;
using IdentityManager.Models.Enums;
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

        public async Task<string> RefreshAccessTokenAsync(string token)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "identityapi"),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", token)
            });

            var response = await _httpClient
                .PostAsync("http://keycloak:8080/realms/AppRealm/protocol/openid-connect/token", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseData);
                return json.RootElement.GetProperty("access_token").GetString();
            }

            return null;
        }
    }
    public interface ITokenService
    { 
        Task<string> GetAdminTokenAsync();
        Task<string> RefreshAccessTokenAsync(string token);
    }
}