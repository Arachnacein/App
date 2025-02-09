using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages
{
    public partial class BudgetPage
    {
        [Inject] private IDialogService dialogService { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private IStringLocalizer<BudgetPage> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private DateTime CurrentDate;
        private PatternViewModel patternViewModel;
        private List<IncomeViewModel> incomes;
        private List<TransactionViewModel> transactions = new List<TransactionViewModel>();
        private PatternValuesModel patternValuesModel = new PatternValuesModel();
        private bool IsLoadingTransactions = true;
        protected override async Task OnInitializedAsync()
        {
            CurrentDate = DateTime.Now;
            IsLoadingTransactions = true;
            await base.OnInitializedAsync();
            await RefreshData();
        }

        private async Task RefreshData()
        {
            if (UserSessionService != null && UserSessionService.UserId != Guid.Empty)
            {
                await ResetModel(patternValuesModel);
                await LoadTransactions();
                await LoadMonthPatterns();
                await LoadMonthIncome();
                await CalculatePatternValues();
                IsLoadingTransactions = false;
            }
        }
        private async Task LoadTransactions()
        {
            try
            {
                transactions = await httpClient.GetFromJsonAsync<List<TransactionViewModel>>($"/api/transaction?userId={UserSessionService.UserId}");
                transactions = transactions.OrderByDescending(x => x.Date)
                                       .Where(x => x.Date.Value.Month == CurrentDate.Month && x.Date.Value.Year == CurrentDate.Year)
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

            await dialogService.ShowAsync<AddTransactionDialog>(Localizer["AddNewTransaction"], parameters, options);
        }
        private async Task AddRecurringTransaction()
        {
            //if(UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            //{
            //    Snackbar.Add(Localizer["MustSignInButton"], Severity.Info);
            //    return;
            //}
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            await dialogService.ShowAsync<AddRecurringTransactionDialog>(Localizer["AddRecurringTransaction"], options);
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
            await dialogService.ShowAsync<AddIncomeDialog>(Localizer["AddNewIncome"], parameters, options);
        }
        private async Task EditDeleteTransaction(TransactionViewModel model)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(RefreshData);

            await dialogService.ShowAsync<EditDeleteTransactionDialog>($"{model.Name}", parameters, options);
        } 
        private async Task ItemUpdated(MudItemDropInfo<TransactionViewModel> dropItem)
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
                return;
            
            //parses string into enum
            dropItem.Item.Category = (TransactionCategoryEnum)Enum.Parse(typeof(TransactionCategoryEnum), dropItem.DropzoneIdentifier);

            await httpClient.PutAsJsonAsync<UpdateTransactionCategoryViewModel>("/api/transaction/UpdateCategory", 
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
            CurrentDate = CurrentDate.AddMonths(-1);
            await RefreshData();
        }
        private async Task NextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            await RefreshData();
        }
        private async Task LoadMonthPatterns()
        {
            var patternResponse = await httpClient.GetFromJsonAsync<PatternViewModel>($"/api/monthpattern/GetMonthPattern?month={CurrentDate.Month}&year={CurrentDate.Year}&userId={UserSessionService.UserId}");
            if (patternResponse == null || patternResponse.Id == -1)
            {
                patternViewModel = new PatternViewModel
                {
                    Id = 0,
                    Name = string.Empty,
                    Value_Saves = 0,
                    Value_Fees = 0,
                    Value_Entertainment = 0
                };
                return;
            }
            
            patternViewModel = patternResponse;
        }
        private async Task LoadMonthIncome()
        {
            var incomeList = await httpClient.GetFromJsonAsync<List<IncomeViewModel>>($"/api/income/GetIncome?userId={UserSessionService.UserId}&month={CurrentDate.Month}&year={CurrentDate.Year}");
            incomes = incomeList;
        }
        private async Task CalculatePatternValues()
        {
            if(patternViewModel != null)
            {
                var incomesTotal = incomes.Sum(x => x.Amount);
                patternValuesModel.TotalValueSaves = incomesTotal * patternViewModel.Value_Saves * 0.01d;
                patternValuesModel.TotalValueFees = incomesTotal * patternViewModel.Value_Fees * 0.01d;
                patternValuesModel.TotalValueEntertainment = incomesTotal * patternViewModel.Value_Entertainment * 0.01d;

                transactions.ForEach(x =>
                {
                    switch (x.Category)
                    {
                        case TransactionCategoryEnum.Saves:
                            patternValuesModel.ActualValueSaves += x.Price;
                            break;
                        case TransactionCategoryEnum.Fees:
                            patternValuesModel.ActualValueFees += x.Price;
                            break;
                        case TransactionCategoryEnum.Entertainment:
                            patternValuesModel.ActualValueEntertainment += x.Price;
                            break;
                        default:
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

            if(patternViewModel != null)
            {
                patternViewModel.Id = 0;
                patternViewModel.Name = "";
                patternViewModel.Value_Saves = 0;
                patternViewModel.Value_Fees = 0;
                patternViewModel.Value_Entertainment = 0;
            }
        }
    }
}