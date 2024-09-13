using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Components.Dialogs
{
    public partial class AddTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DialogModel.Date = DateTime.Now;
        }

        private async Task Submit()
        {
            var request = await httpClient.PostAsJsonAsync<TransactionViewModel>("/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                    snackbar.Add("Successfully added transaction", Severity.Success);

                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
            {
                    snackbar.Add("Failed while adding transaction", Severity.Warning);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}