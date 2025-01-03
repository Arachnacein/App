﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
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
        [Inject] private HttpClient httpClient {  get; set; }
        private string Username { get; set; }
        private string Password { get; set; }
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        InputType PasswordInput = InputType.Password;
        bool isShow;

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

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roles = jwtToken.GetUserRolesFromToken();
                var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
                var surname = jwtToken.Claims.FirstOrDefault(x => x.Type == "surname")?.Value;
                var username = jwtToken.Claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
                var email = jwtToken.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                var userId = Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "userId")?.Value);

                UserSessionService.SetUserSession(token, roles, name, surname, username, email, userId);

                snackbar.Add(Localizer["LogInSuccess"], Severity.Success);
                Navigation.NavigateTo("/",false);
            }
            else
            {
                snackbar.Add(Localizer["LogInError"], Severity.Warning);
            }
        }
        private void ShowPassword()
        {
            if(isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
    }
}