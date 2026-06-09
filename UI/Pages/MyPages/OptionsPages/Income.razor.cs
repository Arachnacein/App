namespace UI.Pages.MyPages.OptionsPages;

public partial class Income
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IStringLocalizer<Income> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    private List<IncomeViewModel> _incomes = new List<IncomeViewModel>();
    private List<IncomeViewModel> _filteredIncomes = new List<IncomeViewModel>();
    private string _searchPhrase = string.Empty;
    public string SearchPhrase
    {
        get => _searchPhrase;
        set                     //for dynamic filtering
        {
            if (_searchPhrase != value)
            {
                _searchPhrase = value;
                FilterIncomes();
            }
        }
    }
    private int IncomesCounter { get; set; } = 0;

    protected override async Task OnInitializedAsync()
    {
        await LoadIncomes();
        IncomesCounter = _incomes.Count();
    }

    private async Task LoadIncomes()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        _incomes = await HttpClient.GetFromJsonAsync<List<IncomeViewModel>>($"/api/income?userId={UserSessionService.UserId}");
        _filteredIncomes = _incomes;
        StateHasChanged();
    }

    private async Task EditIncome(IncomeViewModel model)
    {
        DialogOptions options = new DialogOptions{ CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };
        var parameters = new DialogParameters();
        parameters[nameof(model)] = model;
        parameters["Refresh"] = new Func<Task>(LoadIncomes);

        await DialogService.ShowAsync<EditIncomeDialog>(Localizer["EditTitle"], parameters, options);
    }

    private void FilterIncomes()
    {
        if (string.IsNullOrWhiteSpace(SearchPhrase) || SearchPhrase.Length < 3)
            _filteredIncomes = _incomes;
        else
        {
            _filteredIncomes = _incomes.Where(x =>
                (x.Name?.Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (x.Amount.ToString().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                (x.Date.FormatMY().Contains(SearchPhrase, StringComparison.OrdinalIgnoreCase))
            ).ToList();
        }
        IncomesCounter = _filteredIncomes.Count();
        StateHasChanged();
    }
}