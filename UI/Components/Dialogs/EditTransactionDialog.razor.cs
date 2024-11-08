using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public TransactionViewModel model { get; set;}
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();

        protected override async Task OnInitializedAsync()
        {
            DialogModel = model;
        }
        private async Task Submit()
        {
            var request = await httpClient.PutAsJsonAsync<TransactionViewModel>("/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add("Transaction edited successfully", Severity.Success);
                MudDialog.Cancel();
                if(Refresh != null) 
                    await Refresh.Invoke();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}