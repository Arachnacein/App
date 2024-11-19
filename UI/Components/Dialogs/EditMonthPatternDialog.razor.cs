using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditMonthPatternDialog
    {
        [CascadingParameter] private MudDialogInstance DialogInstance { get; set; }
        [Parameter] public MonthPatternViewModel contextModel { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<EditMonthPatternDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private PatternViewModel patternModel = new PatternViewModel();
        private List<PatternViewModel> patterns = new List<PatternViewModel>();
        

        protected override async Task OnInitializedAsync()
        {
            await LoadPatterns();
        }

        private async Task LoadPatterns()
        {
            if(UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
                return;
            }

            patterns = await httpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");
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