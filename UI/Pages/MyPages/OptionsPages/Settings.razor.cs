namespace UI.Pages.MyPages.OptionsPages;

public partial class Settings
{
    [Inject] private GlobalInfoClass GlobalClass { get; set; }
    [Inject] private IStringLocalizer<Settings> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }

    private string _previewDate = DateTime.Now.ToString("dd.MM");

    private async Task Refresh()
    {
        await SavePreferences();
        Navigation.NavigateTo("/options/settings");
    }

    private async Task OnThemeChanged(RecurringTransactionTheme theme)
    {
        GlobalClass.RecurringTheme = theme;
        await SavePreferences();
    }

    private async Task SavePreferences()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        await HttpClient.PutAsJsonAsync("/api/userpreference", new UserPreferenceViewModel
        {
            UserId = UserSessionService.UserId,
            IsDarkMode = GlobalClass.IsDarkMode,
            RecurringTransactionTheme = (int)GlobalClass.RecurringTheme
        });
    }
}