namespace UI.Components.Dialogs;

public partial class EditMonthPatternDialog
{
    [CascadingParameter] private IMudDialogInstance DialogInstance { get; set; }
    [Parameter] public MonthPatternViewModel ContextModel { get; set; }
    [Parameter] public Func<Task> Refresh { get; set; }
    [Inject] private IStringLocalizer<EditMonthPatternDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private PatternViewModel _patternModel = new PatternViewModel();
    private List<PatternViewModel> _patterns = new List<PatternViewModel>();


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

        _patterns = await HttpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");

        if (_patterns == null)
            Snackbar.Add(Localizer["GettingPatternsError"], Severity.Error);
    }

    private async Task Submit()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }
        var updateModel = new
        {
            Id = ContextModel.Id,
            UserId = UserSessionService.UserId,
            Date = ContextModel.Date,
            PatternId = _patternModel.Id
        };

        var request = await HttpClient.PutAsJsonAsync("/api/monthpattern", updateModel);
        if(request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessEditSnackbar", ContextModel.Date.Month, ContextModel.Date.Year], Severity.Success);
            DialogInstance.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
    }
    private async Task Cancel() => DialogInstance.Cancel();
}
