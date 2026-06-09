namespace UI.Pages.MyPages.OptionsPages;

public partial class Account
{
    [Inject] private IStringLocalizer<Account> Localizer {  get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    private UserDetailsViewModel _userDetails = new UserDetailsViewModel();

    protected override async Task OnInitializedAsync()
    {
        await LoadDataToUserDetailsModel();
    }

    private async Task LoadDataToUserDetailsModel()
    {
        _userDetails.UserId = UserSessionService.UserId;
        _userDetails.FirstName = UserSessionService.Name;
        _userDetails.LastName = UserSessionService.Surname;
        _userDetails.Username = UserSessionService.Username;
        _userDetails.Email = UserSessionService.Email;
        _userDetails.Roles = UserSessionService.Roles
                                .Where(role => role.Contains("user") || role.Contains("admin"))
                                .ToList() ?? new List<string>();
        _userDetails.AccountCreatedDate = UserSessionService.AccountCreatedDate;
        _userDetails.SessionExpiryDate = UserSessionService.TokenExpiryDate;
        _userDetails.EmailVerified = UserSessionService.EmailVerified;

    }
    private async Task EditData(string variableName)
    {
        switch (variableName)
        {
            case nameof(_userDetails.FirstName):
                await OpenDialog(nameof(_userDetails.FirstName));
                break;
            case nameof(_userDetails.LastName):
                await OpenDialog(nameof(_userDetails.LastName));
                break;
            case nameof(_userDetails.Username):
                await OpenDialog(nameof(_userDetails.Username));
                break;
            case nameof(_userDetails.Email):
                await OpenDialog(nameof(_userDetails.Email));
                break;
            default:
                break;
        }
    }

    private async Task OpenDialog(string property)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

        var parameters = new DialogParameters();
        parameters[nameof(property)] = property;
        parameters["UserDetails"] = _userDetails;
        parameters[nameof(OnDialogClose)] = EventCallback.Factory.Create(this, OnDialogClose);

        await DialogService.ShowAsync<EditUserPropertiesDialog>(Localizer["EditUserProperty", Localizer[property]], parameters, options);
    }

    private async Task VerifyEmail()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

        var parameters = new DialogParameters();
        parameters[nameof(OnDialogClose)] = EventCallback.Factory.Create(this, OnDialogClose);

        await DialogService.ShowAsync<VerifyEmailDialog>(String.Empty, parameters, options);

    }

    private async Task OnDialogClose()
    {
        await LoadDataToUserDetailsModel();
        StateHasChanged();
    }
}