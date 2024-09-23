using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;

namespace UI.Components.Dialogs
{
    public partial class EditMonthPatternDialog
    {
        [CascadingParameter] public MudDialogInstance DialogInstance { get; set; }
        [Parameter] public MonthPatternViewModel contextModel { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
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
                snackbar.Add("Error while getting patterns.", Severity.Warning);
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
                snackbar.Add($"Pattern for {contextModel.Date.Month}/{contextModel.Date.Year} updated successfully", Severity.Success);
                DialogInstance.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                snackbar.Add($"Error while updating pattern for {contextModel.Date.Month}/{contextModel.Date.Year}", Severity.Error);
        }
        private async Task Cancel() => DialogInstance.Cancel();
    }
}