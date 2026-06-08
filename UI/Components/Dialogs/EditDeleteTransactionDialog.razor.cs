namespace UI.Components.Dialogs;

public partial class EditDeleteTransactionDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public Func<Task> Refresh { get; set; }
    [Parameter] public TransactionViewModel ParameterModel { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private TransactionViewModelValidator TransactionValidator { get; set; }
    [Inject] private IStringLocalizer<EditDeleteTransactionDialog> Localizer { get; set; }
    private TransactionViewModel _dialogModel = new TransactionViewModel();
    private MudForm _form;

    protected override Task OnInitializedAsync()
    {
        _dialogModel = ParameterModel;
        return base.OnInitializedAsync();
    }
    private async Task Delete()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var request = await HttpClient.DeleteAsync($"/api/transaction/{_dialogModel.Id}/user/{UserSessionService.UserId}");
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessDeleteSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailDeleteSnackbar"], Severity.Error);
    }
    private async Task Edit()
    {
        await _form.ValidateAsync();
        if (!_form.IsValid)
            return;

        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        _dialogModel.UserId = UserSessionService.UserId;
        var request = await HttpClient.PutAsJsonAsync<TransactionViewModel>($"/api/transaction", _dialogModel);

        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessEditSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailEditSnackbar"], Severity.Error);
    }
    private async Task AcceptTransaction()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var request = await HttpClient.PutAsJsonAsync<TransactionViewModel>($"/api/transaction/ConfirmTransaction", _dialogModel);
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessAcceptTransactionSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailAcceptTransactionSnackbar"], Severity.Error);
    }
}