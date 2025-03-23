using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using UI.Extensions;

namespace UI.Pages.MyPages
{
    public partial class Login
    {
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<Login> Localizer { get; set; }
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private MudTextField<string> usernameTextField;
        private string Username { get; set; }
        private string Password { get; set; }
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private InputType PasswordInput = InputType.Password;
        private bool isShow = false;
        private bool isLoading = false;

        private async Task LogIn()
        {
            isLoading = true;
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

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roles = jwtToken.GetUserRolesFromToken();
                var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
                var surname = jwtToken.Claims.FirstOrDefault(x => x.Type == "surname")?.Value;
                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
                var email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                var userId = Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);
                var emailVerifiedClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "email_verified")?.Value;
                var expiryClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                bool emailVerified = bool.TryParse(emailVerifiedClaim, out var result) ? result : false;

                DateTime expiryDate;
                if (long.TryParse(expiryClaim, out var expSeconds))
                    expiryDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                else expiryDate = DateTime.MinValue;

                var accountCreatedDate = jwtToken.Claims.FirstOrDefault(x => x.Type == "created_at")?.Value;
                DateTime createdAt = long.TryParse(accountCreatedDate, out var timestamp)
                                                        ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime
                                                        : DateTime.MinValue;

                UserSessionService.SetUserSession(token, roles, name, surname, username, email, userId, expiryDate, createdAt, emailVerified);

                isLoading = false;
                snackbar.Add(Localizer["LogInSuccess"], Severity.Success);
                Navigation.NavigateTo("/", false);
            }
            else
            {
                isLoading = false;  
                snackbar.Add(Localizer["LogInError"], Severity.Warning);
            }
        }
        private void ShowPassword()
        {
            isShow = !isShow;
            PasswordInputIcon = isShow ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
            PasswordInput = isShow ? InputType.Text : InputType.Password;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if(firstRender)
                await usernameTextField.FocusAsync();
        }
        private async Task HandleEnterDown(KeyboardEventArgs e)
        {
                if (e.Key == "Enter")
                    await LogIn();
        }
    }
}