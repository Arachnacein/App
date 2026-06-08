namespace UI.Components.Dialogs;

public partial class EditIncomeDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public IncomeViewModel ParameterModel { get; set; }
    [Parameter] public Func<Task> Refresh {  get; set; }
    [Inject] private IStringLocalizer<EditIncomeDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private IncomeViewModelValidator IncomeValidator { get; set; }
    private IncomeViewModel _model = new IncomeViewModel();
    private MudForm _form;

    protected override async Task OnInitializedAsync()
    {
        _model = ParameterModel;
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

        _model.UserId = UserSessionService.UserId;
        var request = await HttpClient.PutAsJsonAsync<IncomeViewModel>("/api/income",_model);
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessUpdateSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if(Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailUpdateSnackbar"], Severity.Error);
    }

    private async Task Cancel() => MudDialog.Cancel();

    private async Task Delete()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var request = await HttpClient.DeleteAsync($"/api/income/{_model.Id}/user/{UserSessionService.UserId}");
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailUpdateSnackbar"], Severity.Error);
    }
}