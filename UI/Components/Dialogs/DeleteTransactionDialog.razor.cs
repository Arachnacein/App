namespace UI.Components.Dialogs;

public partial class DeleteTransactionDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public TransactionViewModel ParameterModel { get; set; }
    [Parameter] public Func<Task> Refresh { get; set; }
    [Inject] private ISnackbar snackbar { get; set; }
    [Inject] private IStringLocalizer<DeleteTransactionDialog> Localizer { get; set; }
    [Inject] private HttpClient httpClient { get; set; }
    private TransactionViewModel DialogModel = new TransactionViewModel();

    protected override Task OnInitializedAsync()
    {
        DialogModel = ParameterModel;
        return base.OnInitializedAsync();
    }

    private async Task Submit()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
            return;
        }

        var request = await httpClient.DeleteAsync($"/api/transaction/{DialogModel.Id}/user/{UserSessionService.UserId}");
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);
            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            snackbar.Add(Localizer["FailSnackbar"], Severity.Error);
    }
    private async Task Cancel() => MudDialog.Cancel();
}