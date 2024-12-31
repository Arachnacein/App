using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditUserPropertiesDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Property { get; set; }
        [Parameter] public UserDetailsViewModel UserDetails{ get; set; }
        [Inject] private UserDetailsViewModelValidator UserDetailsValidator { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
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
                Snackbar.Add(/*Localizer["MustSignIn"]*/"dasd", Severity.Warning);
                return;
            }
            var response = await httpClient.PutAsJsonAsync<UserDetailsViewModel>($"/api/editUser", DialogModel);

            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Snackbar.Add(/*Localizer["SuccessEditSnackbar"]*/"sda", Severity.Success);
                MudDialog.Cancel();
            }
            else
                Snackbar.Add(/*Localizer["FailEditSnackbar"]*/$"error {response.StatusCode.ToString()}", Severity.Error);

        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}