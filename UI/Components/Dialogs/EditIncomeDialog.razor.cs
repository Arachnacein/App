using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditIncomeDialog
    {
        [CascadingParameter] MudDialogInstance MudDialogInstance { get; set; }
        [Parameter] public IncomeViewModel model { get; set; }
        [Parameter] public Func<Task> Refresh {  get; set; }
        [Inject] private HttpClient httpClient {  get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] public IStringLocalizer<EditIncomeDialog> Localizer { get; set; }
        [Inject] public IncomeViewModelValidator IncomeValidator { get; set; }
        private IncomeViewModel Model = new IncomeViewModel();

        private MudForm Form;

        protected override async Task OnInitializedAsync()
        {
            Model = model;
        }
        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;

            var request = await httpClient.PutAsJsonAsync<IncomeViewModel>("/api/income",Model);
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add("Income updated successfully", Severity.Success);
                MudDialogInstance.Cancel();
                if(Refresh != null) 
                    await Refresh.Invoke();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task Cancel() => MudDialogInstance.Cancel();
        private async Task Delete()
        {
            var request = await httpClient.DeleteAsync($"/api/income/{Model.Id}");
            if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add("Income deleted successfully", Severity.Success);
                MudDialogInstance.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
    }
}