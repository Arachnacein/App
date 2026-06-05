using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages;

public partial class Settings
{
    [Inject] private GlobalInfoClass _globalClass { get; set; }
    [Inject] private IStringLocalizer<Settings> Localizer { get; set; }
    [Inject] private HttpClient httpClient { get; set; }

    private string _previewDate = DateTime.Now.ToString("dd.MM");

    private async Task Refresh()
    {
        await SavePreferences();
        Navigation.NavigateTo("/options/settings");
    }

    private async Task OnThemeChanged(RecurringTransactionTheme theme)
    {
        _globalClass.RecurringTheme = theme;
        await SavePreferences();
    }

    private async Task SavePreferences()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;
        await httpClient.PutAsJsonAsync("/api/userpreference", new UserPreferenceViewModel
        {
            UserId = UserSessionService.UserId,
            IsDarkMode = _globalClass.IsDarkMode,
            RecurringTransactionTheme = (int)_globalClass.RecurringTheme
        });
    }
}
