namespace UI.Pages.MyPages.OptionsPages;

public partial class Transaction
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private List<TransactionViewModel> _transactions = new List<TransactionViewModel>();
    private List<TransactionViewModel> _filteredTransactions = new List<TransactionViewModel>();
    private string _searchPhrase = string.Empty;

    public string SearchPhrase //This is for dynamic filtering
    {
        get => _searchPhrase;
        set
        {
            if (_searchPhrase != value)
            {
                _searchPhrase = value;
                FilterTransactions();
            }
        }
    }
    private int TransactionCounter { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        await LoadTransactions();
        TransactionCounter = _transactions.Count();
    }

    private async Task LoadTransactions()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        _transactions = await HttpClient.GetFromJsonAsync<List<TransactionViewModel>>($"/api/transaction?userId={UserSessionService.UserId}");
        _filteredTransactions = _transactions;
        StateHasChanged();
    }

    private void FilterTransactions()
    {
        if (string.IsNullOrWhiteSpace(SearchPhrase) || SearchPhrase.Length < 3)
            _filteredTransactions = _transactions;
        else
        {
            _filteredTransactions = _transactions.Where(x =>
                (x.Name?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.Description?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.Category.ToString().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                (x.Price.ToString().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                (x.Date.FormatMY().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
        TransactionCounter = _filteredTransactions.Count();
        StateHasChanged();
    }
    private async Task EditOptions(TransactionViewModel model)
    {
        var parameters = new DialogParameters();
        parameters[nameof(model)] = model;
        parameters["Refresh"] = new Func<Task>(LoadTransactions);
        var options = new DialogOptions { CloseOnEscapeKey = true };

        await DialogService.ShowAsync<EditDeleteTransactionDialog>(Localizer["Options"], parameters, options);

    }
}