using System.Text.Json;

namespace IdentityManager.Services
{
    public class LoginService : ILoginservice
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", "identityapi"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
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
    public interface ILoginservice
    { 
        Task<string> Login(string username, string password);
    }
}