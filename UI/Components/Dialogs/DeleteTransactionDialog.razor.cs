using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class DeleteTransactionDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public IStringLocalizer<DeleteTransactionDialog> Localizer { get; set; }
        [Parameter] public TransactionViewModel model { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }

        protected override Task OnInitializedAsync()
        {
            DialogModel = model;
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {
            var request = await httpClient.DeleteAsync($"/api/transaction/{DialogModel.Id}");
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
