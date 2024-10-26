using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class PatternDialog
    {
        [CascadingParameter] MudDialogInstance MudDialogInstance { get; set; }
        [Parameter] public Func<Task> Refresh {  get; set; }
        [Parameter] public IncomeViewModel DialogModel { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        PatternViewModel model = new PatternViewModel();

        List<PatternViewModel> patterns  = new List<PatternViewModel>();

        protected override async Task OnInitializedAsync()
        {
            await GetPatterns();
        }

        private async Task GetPatterns()
        {
            patterns = await httpClient.GetFromJsonAsync<List<PatternViewModel>>("/api/pattern");
            if (patterns == null)
                snackbar.Add("Error while getting patterns.", Severity.Warning);
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
                snackbar.Add("Please choose pattern", Severity.Warning);

            var addPatternRequest = await httpClient.PostAsJsonAsync("/api/monthpattern",requestBody);
            if (addPatternRequest.StatusCode != System.Net.HttpStatusCode.Created)
                snackbar.Add("Something went wrong", Severity.Error);
            else
                MudDialogInstance.Cancel();
        }
    }
}