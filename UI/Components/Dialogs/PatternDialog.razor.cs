using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class PatternDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialogInstance { get; set; }
        [Parameter] public Func<Task> Refresh {  get; set; }
        [Parameter] public IncomeViewModel DialogModel { get; set; }
        [Inject] private IStringLocalizer<PatternDialog> Localizer { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private PatternViewModel model = new PatternViewModel();
        private List<PatternViewModel> patterns  = new List<PatternViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetPatterns();
        }

        private async Task GetPatterns()
        {
            patterns = await httpClient.GetFromJsonAsync<List<PatternViewModel>>("/api/pattern");
            if (patterns == null)
                snackbar.Add(Localizer["ErrorGettingPatterns"], Severity.Error);
        }

        private async Task AcceptPattern()
        {
            //add pattern api
            var requestBody = new
            {
                Date = new DateTime(DialogModel.Date.Value.Year, DialogModel.Date.Value.Month, DialogModel.Date.Value.Day),
                PatternId = model.Id
            };
            if (model.Id == null)
                snackbar.Add(Localizer["PleaseChoosePattern"], Severity.Warning);

            var addPatternRequest = await httpClient.PostAsJsonAsync("/api/monthpattern",requestBody);
            if (addPatternRequest.StatusCode != System.Net.HttpStatusCode.Created)
                snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
            else
            {
                snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
                MudDialogInstance.Cancel();
            }
        }
    }
}