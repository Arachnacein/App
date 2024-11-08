using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditDeleteTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Parameter] public TransactionViewModel model { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private TransactionViewModelValidator TransactionValidator { get; set; }
        [Inject] private IStringLocalizer<EditDeleteTransactionDialog> Localizer { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();
        private MudForm Form;

        protected override Task OnInitializedAsync()
        {
            DialogModel = model;
            return base.OnInitializedAsync();
        }
        private async Task Delete()
        {
            var request = await httpClient.DeleteAsync($"/api/transaction/{DialogModel.Id}");
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailDeleteSnackbar"], Severity.Error);
        }
        private async Task Edit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;

            var request = await httpClient.PutAsJsonAsync<TransactionViewModel>($"/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessEditSnackbar"], Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}