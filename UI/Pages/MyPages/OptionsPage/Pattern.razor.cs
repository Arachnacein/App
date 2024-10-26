using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Pattern
    {
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        private PatternViewModel model = new PatternViewModel();
        private List<MonthPatternViewModel> patterns = new List<MonthPatternViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadMonthPatterns();
        }
        private async Task AddPattern()
        {
            var request = await httpClient.PostAsJsonAsync<PatternViewModel>("/api/pattern", model);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
                snackbar.Add("Successfully added new pattern.", Severity.Success);
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task LoadMonthPatterns()
        {
            patterns = await httpClient.GetFromJsonAsync<List<MonthPatternViewModel>>("/api/monthpattern/GetAllWithPattern");
            StateHasChanged();
        }
        private async Task EditMonthPattern(MonthPatternViewModel contextModel)
        {
            var parameters = new DialogParameters();
            parameters[nameof(contextModel)] = contextModel;
            parameters["Refresh"] = new Func<Task>(LoadMonthPatterns);
            var options = new DialogOptions { CloseOnEscapeKey = true };

            dialogService.Show<EditMonthPatternDialog>($"Edit pattern for {contextModel.Date.Month}/{contextModel.Date.Year}", parameters, options);
        }
    }
}