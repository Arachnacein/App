namespace UI.Components.Dialogs;

public partial class AddIncomeDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public Func<Task> Refresh {  get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IStringLocalizer<AddIncomeDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private IncomeViewModelValidator IncomeValidator { get; set; }
    private IncomeViewModel _dialogModel = new IncomeViewModel();
    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        _dialogModel.Date = DateTime.Now;
    }

    private async Task Submit()
    {
        await _form.ValidateAsync();
        if (!_form.IsValid)
            return;
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        //check if pattern for this month exists
        var patternResponse = await HttpClient.GetFromJsonAsync<PatternViewModel>
            (@$"/api/monthpattern/GetMonthPattern?month={_dialogModel.Date.Value.Month}
                                                &year={_dialogModel.Date.Value.Year}
                                                &userId={UserSessionService.UserId}");
        if (patternResponse.Id != -1)
        {
            _dialogModel.UserId = UserSessionService.UserId;
            var request = await HttpClient.PostAsJsonAsync<IncomeViewModel>("/api/income", _dialogModel);
            if (request.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
                MudDialog.Cancel();
                if(Refresh != null)
                    await Refresh.Invoke();
            }
            else
                Snackbar.Add(Localizer["FailAddSnacbar"], Severity.Error);
        }
        else
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
            var parameters = new DialogParameters();
            parameters["DialogModel"] = _dialogModel;
            parameters["Refresh"] = new Func<Task>(Refresh);

            var dialogRef = await DialogService.ShowAsync<PatternDialog>(Localizer["ChoosePattern", _dialogModel.Date.Value.Month, _dialogModel.Date.Value.Year], parameters, options);
            var result = await dialogRef.Result;
            if (result is { Canceled: false })
            {
                _dialogModel.UserId = UserSessionService.UserId;
                var request = await HttpClient.PostAsJsonAsync<IncomeViewModel>("/api/income", _dialogModel);
                if (request.StatusCode == HttpStatusCode.Created)
                {
                    Snackbar.Add(Localizer["SuccessAddSnackbar"], Severity.Success);
                    MudDialog.Cancel();
                    if (Refresh != null)
                        await Refresh.Invoke();
                }
                else
                    Snackbar.Add(Localizer["FailAddSnacbar"], Severity.Error);
            }
        }
    }
    private async Task Cancel() => MudDialog.Cancel();
}
