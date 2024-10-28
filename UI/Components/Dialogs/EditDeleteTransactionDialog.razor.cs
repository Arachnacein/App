using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditDeleteTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Parameter] public TransactionViewModel model { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }

        private TransactionViewModel DialogModel = new TransactionViewModel();
        private TransactionViewModelValidator TransactionValidator { get; } = new TransactionViewModelValidator();
        MudForm Form;

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
                snackbar.Add("Transaction deleted successfully.", Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task Edit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;

            var request = await httpClient.PutAsJsonAsync<TransactionViewModel>($"/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add("Transaction edited successfully.", Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}