using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditIncomeDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialogInstance { get; set; }
        [Parameter] public IncomeViewModel model { get; set; }
        [Parameter] public Func<Task> Refresh {  get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<EditIncomeDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient {  get; set; }
        [Inject] private IncomeViewModelValidator IncomeValidator { get; set; }
        private IncomeViewModel Model = new IncomeViewModel();

        private MudForm Form;

        protected override async Task OnInitializedAsync()
        {
            Model = model;
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

            Model.UserId = UserSessionService.UserId;
            var request = await httpClient.PutAsJsonAsync<IncomeViewModel>("/api/income",Model);
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessUpdateSnackbar"], Severity.Success);
                MudDialogInstance.Cancel();
                if(Refresh != null) 
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailUpdateSnackbar"], Severity.Error);
        }
        private async Task Cancel() => MudDialogInstance.Cancel();
        private async Task Delete()
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
                return;
            }

            var request = await httpClient.DeleteAsync($"/api/income/{Model.Id}/user/{UserSessionService.UserId}");
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
                MudDialogInstance.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailUpdateSnackbar"], Severity.Error);
        }
    }
}