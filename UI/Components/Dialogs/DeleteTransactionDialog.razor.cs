using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class DeleteTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public TransactionViewModel model { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<DeleteTransactionDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();

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