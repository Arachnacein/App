using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Pattern
    {
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IDialogService dialogService { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private PatternViewModel Model = new PatternViewModel();
        private List<MonthPatternViewModel> patterns = new List<MonthPatternViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadMonthPatterns();
        }
        private async Task AddPattern()
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
                return;
            
            Model.UserId = UserSessionService.UserId;
            var request = await httpClient.PostAsJsonAsync<PatternViewModel>($"/api/pattern?userId={UserSessionService.UserId}", Model);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
                snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
            else
                snackbar.Add(Localizer["FailAddSnackbar"], Severity.Error);
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

            dialogService.Show<EditMonthPatternDialog>(Localizer["EditPatternDialogHeader", contextModel.Date.Month, contextModel.Date.Year], parameters, options);
        }
    }
}