namespace UI.Components.Dialogs;

public partial class DeleteTransactionDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public TransactionViewModel ParameterModel { get; set; }
    [Parameter] public Func<Task> Refresh { get; set; }
    [Inject] private IStringLocalizer<DeleteTransactionDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private TransactionViewModel _dialogModel = new TransactionViewModel();

    protected override Task OnInitializedAsync()
    {
        _dialogModel = ParameterModel;
        return base.OnInitializedAsync();
    }

    private async Task Submit()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var request = await HttpClient.DeleteAsync($"/api/transaction/{_dialogModel.Id}/user/{UserSessionService.UserId}");
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
    }
    private async Task Cancel() => MudDialog.Cancel();
}
