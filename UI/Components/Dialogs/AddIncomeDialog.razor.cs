using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
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
        [Inject] public IStringLocalizer<AddIncomeDialog> Localizer { get; set; }
        [Inject] public IncomeViewModelValidator IncomeValidator { get; set; }

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
                    snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
                    MudDialog.Cancel();
                    if(Refresh != null) 
                        await Refresh.Invoke();
                }
                else
                    snackbar.Add(Localizer["FailAddSnacbar"], Severity.Error);
            }
            else
            {
                var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
                var parameters = new DialogParameters();
                parameters[nameof(DialogModel)] = DialogModel;
                parameters["Refresh"] = new Func<Task>(Refresh);

                await dialogService.ShowAsync<PatternDialog>(Localizer["ChoosePattern", DialogModel.Date.Value.Month, DialogModel.Date.Value.Year], parameters, options);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}