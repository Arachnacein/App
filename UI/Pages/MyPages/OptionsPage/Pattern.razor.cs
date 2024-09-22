using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Pattern
    {
        [Inject] public HttpClient httpClient { get; set; }
        [Inject]public ISnackbar snackbar { get; set; }
        private PatternViewModel model = new PatternViewModel();
        private List<MonthPatternViewModel> patterns = new List<MonthPatternViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await LoadPatterns();
        }
        private async Task AddPattern()
        {
            var request = await httpClient.PostAsJsonAsync<PatternViewModel>("/api/pattern", model);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
                snackbar.Add("Successfully added new pattern.", Severity.Success);
            else
                snackbar.Add("Something went wrong", Severity.Error);
        }
        private async Task LoadPatterns()
        {
            patterns = await httpClient.GetFromJsonAsync<List<MonthPatternViewModel>>("/api/monthpattern/GetAllWithPattern");
        }
    }
}