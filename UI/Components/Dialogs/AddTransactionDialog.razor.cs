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
            DialogModel.IncomeType = true;
            DialogModel.Date = DateTime.Now;
        }

        private async Task Submit()
        {
            var request = await httpClient.PostAsJsonAsync<TransactionViewModel>("/api/budget", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                if (DialogModel.IncomeType)
                    snackbar.Add("Successfully added income", Severity.Success);
                else
                    snackbar.Add("Successfully added expense", Severity.Success);

                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
            {
                if (DialogModel.IncomeType)
                    snackbar.Add("Failed while adding income", Severity.Warning);
                else
                    snackbar.Add("Failed while added expense", Severity.Warning);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}