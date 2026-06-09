namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages;

public partial class AdminPanelUsersPage
{
    [Inject] private IStringLocalizer<AdminPanel> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }

    private List<UserDetailsViewModel> _users = new List<UserDetailsViewModel>();
    private List<UserDetailsViewModel> _filteredUsers = new List<UserDetailsViewModel>();
    private string _searchPhrase = string.Empty;
    public string SearchPhrase // dynamic filtering
    {
        get => _searchPhrase;
        set
        {
            if (_searchPhrase != value)
            {
                _searchPhrase = value;
                FilterUsers();
            }
        }
    }
    public int UsersCounter { get; set; }
    protected override async Task OnInitializedAsync()
    {
        if (UserSessionService != null && UserSessionService.IsAdmin)
            await FetchUsersAsync();
    }

    private async Task FetchUsersAsync()
    {
        _users = await HttpClient.GetFromJsonAsync<List<UserDetailsViewModel>>("api/User");
        _filteredUsers = _users;
        StateHasChanged();
    }
    private void FilterUsers()
    {
        if (string.IsNullOrWhiteSpace(SearchPhrase) || SearchPhrase.Length < 3)
            _filteredUsers = _users;
        else
        {
            _filteredUsers = _users.Where(x =>
                (x.FirstName?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.LastName?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.Email?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.AccountCreatedDate.FormatMY().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
        UsersCounter = _filteredUsers.Count();
        StateHasChanged();
    }
    private async Task Enable(UserDetailsViewModel model)
    {
        model.Enabled = !model.Enabled;
        await HttpClient.PutAsJsonAsync("api/User/enableUser", model);
    }
}