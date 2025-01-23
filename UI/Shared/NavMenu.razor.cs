using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;

namespace UI.Shared
{
    public partial class NavMenu
    {
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IStringLocalizer<NavMenu> Localizer { get; set; }

        private async void SetPolish() => await SetCulture("pl-PL");
        private async void SetEnglish() => await SetCulture("en-UK");

        private async Task SetCulture(string culture)
        {
            var uri = $"{NavManager.Uri}?culture={culture}";
            await JSRuntime.InvokeVoidAsync("blazorCulture.set", culture);
            NavManager.NavigateTo(uri, forceLoad: true);
        }
        private async Task LogIn()
        {
             Navigation.NavigateTo("/login", false);
        }
        private async Task LogOut()
        {
            UserSessionService.ClearUserSession();
            await localStorage.DeleteAsync("access_token");
            Snackbar.Add(Localizer["LogOutSuccess"], Severity.Success);
            Navigation.NavigateTo("/", false);
        }
        private async Task Register()
        {
            Navigation.NavigateTo("/register", false);
        }        
        private async Task CheckToken()
        {
            Snackbar.Add(UserSessionService.Token, Severity.Normal);
            //Snackbar.Add("Username " + UserSessionService.Username, Severity.Normal);
            //Snackbar.Add("Name " + UserSessionService.Name, Severity.Error);
            //Snackbar.Add("Surname " + UserSessionService.Surname, Severity.Success);
            //Snackbar.Add("Email " + UserSessionService.Email, Severity.Info);
            //Snackbar.Add("Id " + UserSessionService.UserId, Severity.Success);
        }
    }
}