using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Components.Dialogs
{
    public partial class EditTransactionDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Parameter] public TransactionViewModel model { get; set;}
        [Parameter] public Func<Task> Refresh { get; set; }

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
                snackbar.Add("Something went wrong", Severity.Warning);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
