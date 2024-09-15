using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Components.Dialogs
{
    public partial class AddIncomeDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        private IncomeViewModel DialogModel = new IncomeViewModel();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DialogModel.Date = DateTime.Now;
        }

        private async Task Submit()
        {
            var request = await httpClient.PostAsJsonAsync<IncomeViewModel>("/api/income", DialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                snackbar.Add("Successfully added new income", Severity.Success);
                MudDialog.Cancel();
            }
            else
                snackbar.Add("Failed while adding income", Severity.Warning);
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}
