﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models;

namespace UI.Pages.MyPages
{
    public partial class BudgetPage
    {
        private List<TransactionViewModel> transactions = new List<TransactionViewModel>();
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public IDialogService dialogService { get; set; }
        [Inject] public ISnackbar snackbar { get; set; }
        private DateTime CurrentDate;
        private PatternViewModel patternViewModel;
        private List<IncomeViewModel> incomes;

        private PatternValuesModel patternValuesModel = new PatternValuesModel();

        protected override async Task OnInitializedAsync()
        {
            CurrentDate = DateTime.Now;

            await base.OnInitializedAsync();
            await RefreshData();
        }

        private async Task RefreshData()
        {
            await ResetModel(patternValuesModel);
            await LoadTransactions();
            await LoadMonthPatterns();
            await LoadMonthIncome();
            await CalculatePatternValues();
        }
        private async Task LoadTransactions()
        {
            try
            {
                transactions = await httpClient.GetFromJsonAsync<List<TransactionViewModel>>("/api/transaction");
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
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            var parameters = new DialogParameters();
            parameters["Refresh"] = new Func<Task>(RefreshData);

            await dialogService.ShowAsync<AddTransactionDialog>("Add new transaction", parameters, options);
        }
        private async Task AddIncome()
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            var parameters = new DialogParameters();
            parameters["Refresh"] = new Func<Task>(RefreshData);
            await dialogService.ShowAsync<AddIncomeDialog>("Add new income", parameters, options);
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
            //parses string into enum
            dropItem.Item.Category = (TransactionCategoryEnum)Enum.Parse(typeof(TransactionCategoryEnum), dropItem.DropzoneIdentifier);

            await httpClient.PutAsJsonAsync<UpdateTransactionCategoryViewModel>("/api/transaction/UpdateCategory", new UpdateTransactionCategoryViewModel {Id = dropItem.Item.Id, Category = dropItem.Item.Category });
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
            var patternResponse = await httpClient.GetFromJsonAsync<PatternViewModel>($"/api/monthpattern/GetMonthPattern?month={CurrentDate.Month}&year={CurrentDate.Year}");
            if(patternResponse.Id != -1)
                patternViewModel = patternResponse;
        }
        private async Task LoadMonthIncome()
        {
            var incomeList = await httpClient.GetFromJsonAsync<List<IncomeViewModel>>($"/api/income/GetIncome?month={CurrentDate.Month}&year={CurrentDate.Year}");
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
        private class PatternValuesModel
        {
            public double ActualValueSaves { get; set; }
            public double ActualValueFees { get; set; }
            public double ActualValueEntertainment { get; set; }
            public double TotalValueSaves { get; set; }
            public double TotalValueFees { get; set; }
            public double TotalValueEntertainment { get; set; }

        }
    }
}