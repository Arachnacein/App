using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditMonthPatternDialog
    {
        [CascadingParameter] public MudDialogInstance DialogInstance { get; set; }
        [Parameter] public MonthPatternViewModel contextModel { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        [Inject] public IStringLocalizer<EditMonthPatternDialog> Localizer { get; set; }
        PatternViewModel patternModel = new PatternViewModel();
        List<PatternViewModel> patterns = new List<PatternViewModel>();
        

        protected override async Task OnInitializedAsync()
        {
            await LoadPatterns();
        }

        private async Task LoadPatterns()
        {
            patterns = await httpClient.GetFromJsonAsync<List<PatternViewModel>>("/api/pattern");
            if (patterns == null)
                snackbar.Add(Localizer["GettingPatternsError"], Severity.Error);
        }
        private async Task Submit()
        {
            var updateModel = new
            {
                Id = contextModel.Id,
                Date = contextModel.Date,
                PatternId = patternModel.Id
            };

            var request = await httpClient.PutAsJsonAsync("/api/monthpattern", updateModel);
            if(request.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                snackbar.Add(Localizer["SuccessEditSnackbar", contextModel.Date.Month, contextModel.Date.Year], Severity.Success);
                DialogInstance.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
        }
        private async Task Cancel() => DialogInstance.Cancel();
    }
}