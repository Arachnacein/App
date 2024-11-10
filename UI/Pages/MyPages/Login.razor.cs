using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using System.Text.Json;

namespace UI.Pages.MyPages
{
    public partial class Login
    {
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        [Inject] private HttpClient httpClient {  get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        private async Task LogIn()
        {
            var requestBody = new FormUrlEncodedContent(new[] //keycloak needs x-www-form-urlencoded format
            {
                new KeyValuePair<string, string>("username", Username),
                new KeyValuePair<string, string>("password", Password)
            });

            var response = await httpClient.PostAsync("/api/login", requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(responseData);
                var token = json.RootElement.GetProperty("access_token").GetString();

                await localStorage.SetAsync("access_token", token);

                snackbar.Add("Zalogowano pomyslnie", Severity.Success);
                Navigation.NavigateTo("/",false);
            }
            else
            {
                snackbar.Add("cos nie tak", Severity.Warning);
            }
        }
    }
}