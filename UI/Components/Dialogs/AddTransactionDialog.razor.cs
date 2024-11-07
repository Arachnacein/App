using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private TransactionViewModel DialogModel = new TransactionViewModel();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] public IStringLocalizer<AddTransactionDialog> Localizer { get; set; }
        [Inject] public TransactionViewModelValidator TransactionValidator { get; set; }
        private MudForm Form;

        protected override async Task OnInitializedAsync()
        {
            DialogModel.Date = DateTime.Now;
        }

        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;

            var request = await httpClient.PostAsJsonAsync<TransactionViewModel>("/api/transaction", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);

                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
            {
                snackbar.Add(Localizer["FailedSnackbar"], Severity.Warning);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}