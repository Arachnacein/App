namespace UI.Components.Dialogs;

public partial class PatternDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public Func<Task> Refresh {  get; set; }
    [Parameter] public IncomeViewModel ParameterModel { get; set; }
    [Inject] private IStringLocalizer<PatternDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private PatternViewModel _model = new PatternViewModel();
    private List<PatternViewModel> _patterns = new List<PatternViewModel>();

    protected override async Task OnInitializedAsync()
    {
        await GetPatterns();
    }

    private async Task GetPatterns()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        _patterns = await HttpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");

        if (_patterns == null)
            Snackbar.Add(Localizer["ErrorGettingPatterns"], Severity.Error);
    }

    private async Task AcceptPattern()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }
        var requestBody = new
        {
            UserId = UserSessionService.UserId,
            Date = new DateTime(ParameterModel.Date.Value.Year, ParameterModel.Date.Value.Month, ParameterModel.Date.Value.Day),
            PatternId = _model.Id
        };

        if (_model.Id == null)
        {
            Snackbar.Add(Localizer["PleaseChoosePattern"], Severity.Warning);
            return;
        }

        var addPatternRequest = await HttpClient.PostAsJsonAsync("/api/monthpattern",requestBody);

        if (addPatternRequest.StatusCode != HttpStatusCode.Created)
            Snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
        else
        {
            Snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
            MudDialog.Close();
        }
    }
}
