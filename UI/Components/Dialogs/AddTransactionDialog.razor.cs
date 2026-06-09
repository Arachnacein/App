namespace UI.Components.Dialogs;

public partial class AddTransactionDialog
{
    [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
    [Parameter] public Func<Task> Refresh { get; set; }
    [Inject] private IStringLocalizer<AddTransactionDialog> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private TransactionViewModelValidator TransactionValidator { get; set; }
    private MudForm _form;
    private TransactionViewModel _dialogModel = new TransactionViewModel();

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

        _dialogModel.UserId = UserSessionService.UserId;
        var request = await HttpClient.PostAsJsonAsync<TransactionViewModel>("/api/transaction", _dialogModel);
        if (request.StatusCode == HttpStatusCode.Created)
        {
            Snackbar.Add(Localizer["SuccessSnackbar"], Severity.Success);

            MudDialog.Cancel();
            if (Refresh != null)
                await Refresh.Invoke();
        }
        else
            Snackbar.Add(Localizer["FailedSnackbar"], Severity.Error);
    }
    private async Task Cancel() => MudDialog.Cancel();
}
