namespace UI.Components.Dialogs;

public partial class PatternDialog
{
    [CascadingParameter] private IMudDialogInstance IMudDialogInstance { get; set; }
    [Parameter] public Func<Task> Refresh {  get; set; }
    [Parameter] public IncomeViewModel ParameterModel { get; set; }
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
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        patterns = await httpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");

        if (patterns == null)
            snackbar.Add(Localizer["ErrorGettingPatterns"], Severity.Error);
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
            PatternId = model.Id
        };

        if (model.Id == null)
        {
            snackbar.Add(Localizer["PleaseChoosePattern"], Severity.Warning);
            return;
        }

        var addPatternRequest = await httpClient.PostAsJsonAsync("/api/monthpattern",requestBody);

        if (addPatternRequest.StatusCode != HttpStatusCode.Created)
            snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
        else
        {
            snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
            IMudDialogInstance.Close();
        }
    }
}