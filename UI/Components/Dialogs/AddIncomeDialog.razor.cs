using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddIncomeDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public Func<Task> Refresh {  get; set; }
        private IncomeViewModel DialogModel = new IncomeViewModel();
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        private IncomeViewModelValidation IncomeValidator { get; } = new IncomeViewModelValidation();
        MudForm Form;

        protected override async Task OnInitializedAsync()
        {
            DialogModel.Date = DateTime.Now;
        }

        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;

            //check if pattern for this month exists
            var patternResponse = await httpClient.GetFromJsonAsync<PatternViewModel>($"/api/monthpattern/GetMonthPattern?month={DialogModel.Date.Value.Month}&year={DialogModel.Date.Value.Year}");
            if (patternResponse.Id != -1)
            {
                var request = await httpClient.PostAsJsonAsync<IncomeViewModel>("/api/income", DialogModel);
                if (request.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    snackbar.Add("Successfully added new income", Severity.Success);
                    MudDialog.Cancel();
                    if(Refresh != null) 
                        await Refresh.Invoke();
                }
                else
                    snackbar.Add("Failed while adding income", Severity.Warning);
            }
            else
            {
                var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
                var parameters = new DialogParameters();
                parameters[nameof(DialogModel)] = DialogModel;
                parameters["Refresh"] = new Func<Task>(Refresh);

                await dialogService.ShowAsync<PatternDialog>($"Choose pattern for {DialogModel.Date.Value.Month}/{DialogModel.Date.Value.Year}", parameters, options);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}