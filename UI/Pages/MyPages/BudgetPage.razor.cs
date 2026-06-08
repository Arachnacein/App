namespace UI.Pages.MyPages;

public partial class BudgetPage
{
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IStringLocalizer<BudgetPage> Localizer { get; set; }
    [Inject] private HttpClient HttpClient { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }
    [Inject] private GlobalInfoClass GlobalInfo { get; set; }
    private DateTime _currentDate;
    private PatternViewModel _patternViewModel;
    private List<IncomeViewModel> _incomes;
    private List<TransactionViewModel> _transactions = new List<TransactionViewModel>();
    private PatternValuesModel _patternValuesModel = new PatternValuesModel();
    private bool _isLoadingTransactions = true;
    private double TotalIncome => _incomes?.Sum(x => x.Amount) ?? 0;

    protected override async Task OnInitializedAsync()
    {
        _currentDate = DateTime.Now;
        _isLoadingTransactions = true;
        await base.OnInitializedAsync();
        await RefreshData();
    }

    private async Task RefreshData()
    {
        if (UserSessionService != null && UserSessionService.UserId != Guid.Empty)
        {
            await ResetModel(_patternValuesModel);
            await LoadTransactions();
            await LoadMonthPatterns();
            await LoadMonthIncome();
            await CalculatePatternValues();
            _isLoadingTransactions = false;
            StateHasChanged();
        }
    }
    private async Task LoadTransactions()
    {
        try
        {
            _transactions = await HttpClient.GetFromJsonAsync<List<TransactionViewModel>>
                ($"/api/transaction?userId={UserSessionService.UserId}");

            _transactions = _transactions.OrderByDescending(x => x.Date)
                                   .Where(x => x.Date.Value.Month == _currentDate.Month &&
                                               x.Date.Value.Year == _currentDate.Year)
                                   .ToList();
            StateHasChanged();
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine($"HttpRequestException occured. Message:{e.Message}");
        }
        catch(Exception e)
        {
            Console.WriteLine($"Other error occured. Message:{e.Message}");
        }
    }
    private async Task AddTransaction()
    {
        if(UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignInButton"], Severity.Info);
            return;
        }
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

        var parameters = new DialogParameters();
        parameters["Refresh"] = new Func<Task>(RefreshData);

        await DialogService.ShowAsync<AddTransactionDialog>(Localizer["AddNewTransaction"], parameters, options);
    }
    private async Task AddRecurringTransaction()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignInButton"], Severity.Info);
            return;
        }
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

        var parameters = new DialogParameters();
        parameters["Refresh"] = new Func<Task>(RefreshData);

        await DialogService.ShowAsync<AddRecurringTransactionDialog>(Localizer["AddRecurringTransaction"],parameters, options);
    }
    private async Task AddIncome()
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
        {
            Snackbar.Add(Localizer["MustSignInButton"], Severity.Info);
            return;
        }
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

        var parameters = new DialogParameters();
        parameters["Refresh"] = new Func<Task>(RefreshData);
        await DialogService.ShowAsync<AddIncomeDialog>(Localizer["AddNewIncome"], parameters, options);
    }
    private async Task EditDeleteTransaction(TransactionViewModel model)
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
        var parameters = new DialogParameters();
        parameters[nameof(model)] = model;
        parameters["Refresh"] = new Func<Task>(RefreshData);

        await DialogService.ShowAsync<EditDeleteTransactionDialog>($"{model.Name}", parameters, options);
    }
    private async Task ItemUpdated(MudItemDropInfo<TransactionViewModel> dropItem)
    {
        if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            return;

        //parses string into enum
        var droppedItem = (TransactionCategoryEnum)Enum.
            Parse(typeof(TransactionCategoryEnum), dropItem.DropzoneIdentifier);

        if (dropItem.Item.Category == droppedItem)
            return;

        dropItem.Item.Category = droppedItem;
        await HttpClient.PutAsJsonAsync<UpdateTransactionCategoryViewModel>
            ("/api/transaction/UpdateCategory",
                new UpdateTransactionCategoryViewModel
                {
                    Id = dropItem.Item.Id,
                    UserId = UserSessionService.UserId,
                    Category = dropItem.Item.Category
                });
        await RefreshData();
    }

    private async Task PreviousMonth()
    {
        _currentDate = _currentDate.AddMonths(-1);
        await RefreshData();
        await JSRuntime.InvokeVoidAsync("window.scrollTo", new { top = 0, behavior = "smooth" });
    }

    private async Task NextMonth()
    {
        _currentDate = _currentDate.AddMonths(1);
        await RefreshData();
        await JSRuntime.InvokeVoidAsync("window.scrollTo", new { top = 0, behavior = "smooth" });
    }

    private string GetBarClass(TransactionViewModel t)
        => GlobalInfo.RecurringTheme != RecurringTransactionTheme.RightBar ? "" :
           t.IsRecurring && !t.IsApproved ? " recurring-bar-pending" :
           t.IsRecurring && t.IsApproved  ? " recurring-bar-approved" : "";

    private string? GetBorderStyle(TransactionViewModel t)
        => GlobalInfo.RecurringTheme != RecurringTransactionTheme.Border ? null :
           t.IsRecurring && !t.IsApproved ? "border:1px dashed rgba(167,139,250,0.5)!important;" :
           t.IsRecurring && t.IsApproved  ? "border:1px solid #10B981!important;" : null;

    private async Task LoadMonthPatterns()
    {
        var patternResponse = await HttpClient.GetFromJsonAsync<PatternViewModel>
            ($"/api/monthpattern/GetMonthPattern?month={_currentDate.Month}&year={_currentDate.Year}&userId={UserSessionService.UserId}");
        if (patternResponse == null || patternResponse.Id == -1)
        {
            _patternViewModel = new PatternViewModel
            {
                Id = 0,
                Name = string.Empty,
                Value_Saves = 0,
                Value_Fees = 0,
                Value_Entertainment = 0
            };
            return;
        }

        _patternViewModel = patternResponse;
    }

    private async Task LoadMonthIncome()
    {
        var incomeList = await HttpClient.GetFromJsonAsync<List<IncomeViewModel>>
            ($"/api/income/GetIncome?userId={UserSessionService.UserId}&month={_currentDate.Month}&year={_currentDate.Year}");
        _incomes = incomeList;
    }

    private async Task CalculatePatternValues()
    {
        if(_patternViewModel != null)
        {
            var incomesTotal = _incomes.Sum(x => x.Amount);
            _patternValuesModel.TotalValueSaves = incomesTotal * _patternViewModel.Value_Saves * 0.01d;
            _patternValuesModel.TotalValueFees = incomesTotal * _patternViewModel.Value_Fees * 0.01d;
            _patternValuesModel.TotalValueEntertainment = incomesTotal * _patternViewModel.Value_Entertainment * 0.01d;

            _transactions.ForEach(x =>
            {
                if(!x.IsRecurring || (x.IsRecurring && x.IsApproved))
                switch (x.Category)
                {
                    case TransactionCategoryEnum.Saves:
                        _patternValuesModel.ActualValueSaves += x.Price;
                        break;
                    case TransactionCategoryEnum.Fees:
                        _patternValuesModel.ActualValueFees += x.Price;
                        break;
                    case TransactionCategoryEnum.Entertainment:
                        _patternValuesModel.ActualValueEntertainment += x.Price;
                        break;
                    default:
                        break;
                }

                if (x.IsRecurring && x.IsApproved)
                    switch (x.Category)
                    {
                        case TransactionCategoryEnum.Saves:
                            _patternValuesModel.RecurringValueSaves += x.Price;
                            break;
                        case TransactionCategoryEnum.Fees:
                            _patternValuesModel.RecurringValueFees += x.Price;
                            break;
                        case TransactionCategoryEnum.Entertainment:
                            _patternValuesModel.RecurringValueEntertainment += x.Price;
                            break;
                    }
            });
        }
    }
    private async Task ResetModel(PatternValuesModel model)
    {
        model.ActualValueSaves = 0;
        model.ActualValueFees = 0;
        model.ActualValueEntertainment = 0;
        model.TotalValueSaves = 0;
        model.TotalValueFees = 0;
        model.TotalValueEntertainment = 0;
        model.RecurringValueSaves = 0;
        model.RecurringValueFees = 0;
        model.RecurringValueEntertainment = 0;

        if(_patternViewModel != null)
        {
            _patternViewModel.Id = 0;
            _patternViewModel.Name = "";
            _patternViewModel.Value_Saves = 0;
            _patternViewModel.Value_Fees = 0;
            _patternViewModel.Value_Entertainment = 0;
        }
    }
}