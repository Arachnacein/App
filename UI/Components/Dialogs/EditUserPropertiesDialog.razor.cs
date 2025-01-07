using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using UI.Extensions;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditUserPropertiesDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Property { get; set; }
        [Parameter] public UserDetailsViewModel UserDetails{ get; set; }
        [Inject] private UserDetailsViewModelValidator UserDetailsValidator { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        private UserDetailsViewModel DialogModel = new UserDetailsViewModel();
        private MudForm Form;

        protected override Task OnInitializedAsync()
        {
            DialogModel = UserDetails;
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(/*Localizer["MustSignIn"]*/"user session is not set", Severity.Warning);
                return;
            }
            var responseEditUserProperties = await httpClient.PutAsJsonAsync<UserDetailsViewModel>($"/api/User/editUser", DialogModel);

            if (responseEditUserProperties.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var requestRefreshToken = await httpClient.PostAsync($"/api/User/refreshToken?token={UserSessionService.Token}", null);

                if (requestRefreshToken.IsSuccessStatusCode)
                {
                    UserSessionService.ClearUserSession();

                    var responseData = await requestRefreshToken.Content.ReadAsStringAsync();
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

                    var expiryClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "exp")?.Value;
                    DateTime expiryDate;
                    if (long.TryParse(expiryClaim, out var expSeconds))
                        expiryDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                    else expiryDate = DateTime.MinValue;

                    var accountCreatedDate = jwtToken.Claims.FirstOrDefault(x => x.Type == "created_at")?.Value;
                    DateTime createdAt = long.TryParse(accountCreatedDate, out var timestamp)
                                                            ? DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime
                                                            : DateTime.MinValue;

                    UserSessionService.SetUserSession(token, roles, name, surname, username, email, userId, expiryDate, createdAt);

                    Snackbar.Add(/*Localizer["LogInSuccess"]*/$"success - name from USS {UserSessionService.Name}", Severity.Success);
                    Snackbar.Add($"success - name {name}", Severity.Success);
                    Navigation.NavigateTo("/", false);
                }
                else
                {
                    Snackbar.Add(/*Localizer["LogInError"]*/"error 1", Severity.Warning);
                }

                MudDialog.Cancel();
            }
            else
                Snackbar.Add(/*Localizer["FailEditSnackbar"]*/$"error 2 {responseEditUserProperties.StatusCode.ToString()}", Severity.Error);
            
            
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}