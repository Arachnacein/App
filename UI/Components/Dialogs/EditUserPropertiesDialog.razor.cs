namespace UI.Components.Dialogs;

public partial class EditUserPropertiesDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public string Property { get; set; }
    [Parameter] public UserDetailsViewModel UserDetails { get; set; }
    [Parameter] public EventCallback OnDialogClose { get; set; }
    [Inject] public IStringLocalizer<EditUserPropertiesDialog> Localizer { get; set; }
    [Inject] private UserDetailsViewModelValidator UserDetailsValidator { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] protected ProtectedLocalStorage LocalStorage { get; set; }
    private UserDetailsViewModel _dialogModel = new UserDetailsViewModel();
    private MudForm _form;

    protected override Task OnInitializedAsync()
    {
        _dialogModel = UserDetails;
        return base.OnInitializedAsync();
    }

    private async Task Submit()
    {
        await _form.ValidateAsync();

        if (!_form.IsValid)
            return;

        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var responseEditUserProperties = await HttpClient.PutAsJsonAsync<UserDetailsViewModel>($"/api/User/editUser", _dialogModel);
        if (responseEditUserProperties.StatusCode != System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
            return;
        }

        var refreshedUserData = await HttpClient.GetFromJsonAsync<UserDetailsViewModel>($"/api/User/getUserData?userId={UserSessionService.UserId}");
        UserDetails.FirstName = refreshedUserData.FirstName;
        UserDetails.LastName = refreshedUserData.LastName;
        UserDetails.Username = refreshedUserData.Username;
        UserDetails.Email = refreshedUserData.Email;

        UserSessionService.SetUserSession(UserDetails.FirstName, UserDetails.LastName, UserDetails.Username, UserDetails.Email);
        Snackbar.Add(Localizer["LogInSuccess"], Severity.Success);
        StateHasChanged();

        await OnDialogClose.InvokeAsync();
        MudDialog.Cancel();
    }

    private async Task Cancel() => MudDialog.Cancel();
}
