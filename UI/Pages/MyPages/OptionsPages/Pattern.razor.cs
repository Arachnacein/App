namespace UI.Pages.MyPages.OptionsPages;

public partial class Pattern
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private PatternViewModelValidator PatternValidator { get; set; }
    private MudForm _form;
    private PatternViewModel _model = new PatternViewModel();
    private List<MonthPatternViewModel> _patterns = new List<MonthPatternViewModel>();
    private List<PatternViewModel> _patternsList = new List<PatternViewModel>();

    protected override async Task OnInitializedAsync()
    {
        await LoadMonthPatterns();
        await LoadPatterns();
    }

    private async Task LoadPatterns()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        _patternsList = await HttpClient.GetFromJsonAsync<List<PatternViewModel>>($"/api/pattern?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task AddPattern()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        await _form.ValidateAsync();

        if (!_form.IsValid)
            return;

        if (_model.Value_Saves + _model.Value_Fees + _model.Value_Entertainment != 100)
        {
            Snackbar.Add(Localizer["FailAddSnackbarInvalidValues"], Severity.Warning);
            return;
        }

        _model.UserId = UserSessionService.UserId;
        var request = await HttpClient.PostAsJsonAsync<PatternViewModel>($"/api/pattern?userId={UserSessionService.UserId}", _model);
        if (request.StatusCode == HttpStatusCode.Created)
        {
            Snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
            await LoadPatterns();
        }
        else if (request.StatusCode == HttpStatusCode.UnprocessableEntity)
            Snackbar.Add(Localizer["FailAddSnackbarDuplicate"], Severity.Warning);
        else if (request.StatusCode == HttpStatusCode.Conflict)
            Snackbar.Add(Localizer["FailAddSnackbarInvalidValues"], Severity.Warning);
        else
            Snackbar.Add(Localizer["FailAddSnackbar"], Severity.Error);
    }

    private async Task DeletePattern(PatternViewModel pattern)
    {
        var confirmed = await DialogService.ShowMessageBoxAsync(
            Localizer["DeleteConfirmTitle"],
            $"{pattern.Name}?",
            yesText: Localizer["DeleteConfirmYes"],
            cancelText: Localizer["DeleteConfirmCancel"]);

        if (confirmed != true)
            return;

        var request = await HttpClient.DeleteAsync($"/api/pattern/{pattern.Id}/user/{UserSessionService.UserId}");
        if (request.IsSuccessStatusCode)
        {
            Snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
            await LoadPatterns();
        }
        else if (request.StatusCode == HttpStatusCode.Conflict)
            Snackbar.Add(Localizer["FailDeleteSnackbarInUse"], Severity.Warning);
        else
            Snackbar.Add(Localizer["FailDeleteSnackbar"], Severity.Error);
    }

    private async Task LoadMonthPatterns()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        _patterns = await HttpClient.GetFromJsonAsync<List<MonthPatternViewModel>>($"/api/monthpattern/GetAllWithPattern?userId={UserSessionService.UserId}");
        StateHasChanged();
    }

    private async Task EditMonthPattern(MonthPatternViewModel contextModel)
    {
        var parameters = new DialogParameters();
        parameters[nameof(contextModel)] = contextModel;
        parameters["Refresh"] = new Func<Task>(LoadMonthPatterns);
        var options = new DialogOptions { CloseOnEscapeKey = true };

        await DialogService.ShowAsync<EditMonthPatternDialog>(Localizer["EditPatternDialogHeader", contextModel.Date.Month, contextModel.Date.Year], parameters, options);
    }
}