namespace UI.Shared;

public partial class NavMenu
{
    [Inject] protected ProtectedLocalStorage LocalStorage { get; set; }
    [Inject] private NavigationManager NavManager { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }
    [Inject] private IStringLocalizer<NavMenu> Localizer { get; set; }
    [Inject] private GlobalInfoClass GlobalInfo { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private string _remainingTime;
    private Timer Timer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if(UserSessionService.IsUserLoggedIn())
        {
            _remainingTime = UserSessionService.GetRemainingTime();
            Timer = new Timer(UpdateRemainingTime, null, 0, 1000);
            await LoadUserPreferences();
        }
    }

    private async Task LoadUserPreferences()
    {
        try
        {
            var prefs = await HttpClient.GetFromJsonAsync<UserPreferenceViewModel>(
                $"/api/userpreference?userId={UserSessionService.UserId}");
            if (prefs is not null)
            {
                GlobalInfo.IsDarkMode = prefs.IsDarkMode;
                GlobalInfo.RecurringTheme = (RecurringTransactionTheme)prefs.RecurringTransactionTheme;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load user preferences: {ex.Message}");
        }
    }

    private async Task SetPolish() => await SetCulture("pl-PL");
    private async Task SetEnglish() => await SetCulture("en-UK");
    private async Task Register() => Navigation.NavigateTo("/register", false);
    private async Task LogIn() => Navigation.NavigateTo("/login", false);

    private async Task SetCulture(string culture)
    {
        var uri = $"{NavManager.Uri}?culture={culture}";
        await JSRuntime.InvokeVoidAsync("blazorCulture.set", culture);
        NavManager.NavigateTo(uri, forceLoad: true);
    }

    private async Task LogOut()
    {
        UserSessionService.ClearUserSession();
        await LocalStorage.DeleteAsync("access_token");
        Snackbar.Add(Localizer["LogOutSuccess"], Severity.Success);
        Navigation.NavigateTo("/", false);
    }

    private void UpdateRemainingTime(object state)
    {
        _remainingTime = UserSessionService.GetRemainingTime();
        InvokeAsync(StateHasChanged);
    }
}