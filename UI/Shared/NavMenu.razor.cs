using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace UI.Shared
{
    public partial class NavMenu
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        private async void SetPolish() => await SetCulture("pl-PL");
        private async void SetEnglish() => await SetCulture("en-UK");

        private async Task SetCulture(string culture)
        {
            var uri = $"{NavManager.Uri}?culture={culture}";
            await JSRuntime.InvokeVoidAsync("blazorCulture.set", culture);
            NavManager.NavigateTo(uri, forceLoad: true);
        }
    }
}