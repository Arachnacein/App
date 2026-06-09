namespace UI.Pages.MyPages;

public partial class Login
{
    [Inject] private IStringLocalizer<Login> Localizer { get; set; }
    [Inject] protected ProtectedLocalStorage LocalStorage { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private GlobalInfoClass GlobalInfo { get; set; }
    private MudTextField<string> _usernameTextField;
    private string Username { get; set; }
    private string Password { get; set; }
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
    private InputType _passwordInput = InputType.Password;
    private bool _isShow = false;
    private bool _isLoading = false;

    private async Task LogIn()
    {
        _isLoading = true;
        var requestBody = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("username", Username),
            new KeyValuePair<string, string>("password", Password)
        });

        var response = await HttpClient.PostAsync("/api/login", requestBody);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseData);
            var token = json.RootElement.GetProperty("access_token").GetString();

            await LocalStorage.SetAsync("access_token", token);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roles = jwtToken.GetUserRolesFromToken();
            var name = jwtToken.Claims.FirstOrDefault(x => x.Type == "given_name")?.Value;
            var surname = jwtToken.Claims.FirstOrDefault(x => x.Type == "family_name")?.Value;
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

            await LoadUserPreferences(userId);

            _isLoading = false;
            Snackbar.Add(Localizer["LogInSuccess"], Severity.Success);
            Navigation.NavigateTo("/", false);
        }
        else
        {
            _isLoading = false;
            Snackbar.Add(Localizer["LogInError"], Severity.Warning);
        }
    }

    private async Task LoadUserPreferences(Guid userId)
    {
        try
        {
            var prefs = await HttpClient.GetFromJsonAsync<UserPreferenceViewModel>
                ($"/api/userpreference?userId={userId}");

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

    private void ShowPassword()
    {
        _isShow = !_isShow;
        _passwordInputIcon = _isShow ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
        _passwordInput = _isShow ? InputType.Text : InputType.Password;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
            await _usernameTextField.FocusAsync();
    }

    private async Task HandleEnterDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
            await LogIn();
    }
}