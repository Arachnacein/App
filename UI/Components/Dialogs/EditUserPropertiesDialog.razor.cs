using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditUserPropertiesDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Property { get; set; }
        [Parameter] public UserDetailsViewModel UserDetails { get; set; }
        [Parameter] public EventCallback OnDialogClose { get; set; }
        [Inject] public IStringLocalizer<EditUserPropertiesDialog> Localizer { get; set; }
        [Inject] private UserDetailsViewModelValidator UserDetailsValidator { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] protected ProtectedLocalStorage localStorage { get; set; }
        private UserDetailsViewModel DialogModel = new UserDetailsViewModel();
        private MudForm Form;

        protected override Task OnInitializedAsync()
        {
            DialogModel = UserDetails;
            return base.OnInitializedAsync();
        }

        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
                return;
            }

            var responseEditUserProperties = await httpClient.PutAsJsonAsync<UserDetailsViewModel>($"/api/User/editUser", DialogModel);
            if (responseEditUserProperties.StatusCode != System.Net.HttpStatusCode.NoContent)
            {
                Snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
                return;
            }

            var refreshedUserData = await httpClient.GetFromJsonAsync<UserDetailsViewModel>($"/api/User/getUserData?userId={UserSessionService.UserId}");
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
}