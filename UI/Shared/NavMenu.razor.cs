using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;

namespace UI.Shared
{
    public partial class NavMenu
    {
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

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
            Snackbar.Add("Pomyślnie wylogowano", MudBlazor.Severity.Success);
            Navigation.NavigateTo("/", false);
        }
        private async Task Register()
        {
            
        }        
        private async Task CheckToken()
        {
            Snackbar.Add("Username " + UserSessionService.Username, MudBlazor.Severity.Normal);
            Snackbar.Add("Name " + UserSessionService.Name, MudBlazor.Severity.Error);
            Snackbar.Add("Surname " + UserSessionService.Surname, MudBlazor.Severity.Success);
            Snackbar.Add("Email " + UserSessionService.Email, MudBlazor.Severity.Info);
            Snackbar.Add("Id " + UserSessionService.UserId, MudBlazor.Severity.Success);
        }
    }
}