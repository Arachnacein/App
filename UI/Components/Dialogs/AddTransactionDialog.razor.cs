using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<AddTransactionDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private TransactionViewModelValidator TransactionValidator { get; set; }
        private MudForm Form;
        private TransactionViewModel DialogModel = new TransactionViewModel();

        protected override async Task OnInitializedAsync()
        {
            DialogModel.Date = DateTime.Now;
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

            DialogModel.UserId = UserSessionService.UserId;
            var request = await httpClient.PostAsJsonAsync<TransactionViewModel>("/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);

                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailedSnackbar"], Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}