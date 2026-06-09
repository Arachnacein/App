namespace UI.Components.Dialogs;

public partial class EditTransactionDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public TransactionViewModel ParameterModel { get; set;}
    [Parameter] public Func<Task> Refresh { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private TransactionViewModel _dialogModel = new TransactionViewModel();

    protected override async Task OnInitializedAsync()
    {
        _dialogModel = ParameterModel;
    }

    private async Task Submit()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add("You must sign in");
            return;
        }

        var request = await HttpClient.PutAsJsonAsync<TransactionViewModel>("/api/transaction", _dialogModel);
        if (request.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            Snackbar.Add("Transaction edited successfully", Severity.Success);
            MudDialog.Cancel();
            if(Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add("Something went wrong", Severity.Error);
    }

    private async Task Cancel() => MudDialog.Cancel();
}
