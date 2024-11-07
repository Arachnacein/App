﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Extensions;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Income
    {
        [Inject] HttpClient httpClient {  get; set; }
        [Inject] IDialogService dialogService { get; set; }
        [Inject] IStringLocalizer<Income> Localizer { get; set; }
        private List<IncomeViewModel> incomes = new List<IncomeViewModel>();
        private List<IncomeViewModel> filteredIncomes = new List<IncomeViewModel>();
        private string _searchPhrase = string.Empty;
        public string searchPhrase 
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
            IncomesCounter = incomes.Count();
        }

        private async Task LoadIncomes()
        {
            incomes = await httpClient.GetFromJsonAsync<List<IncomeViewModel>>("/api/income");
            filteredIncomes = incomes;
            StateHasChanged();
        }
        private async Task EditIncome(IncomeViewModel model)
        {
            DialogOptions options = new DialogOptions{ CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(LoadIncomes);

            await dialogService.ShowAsync<EditIncomeDialog>(Localizer["EditTitle"], parameters, options);
        }
        private void FilterIncomes()
        {
            if (string.IsNullOrWhiteSpace(searchPhrase) || searchPhrase.Length < 3)
                filteredIncomes = incomes;
            else
            {
                filteredIncomes = incomes.Where(x =>
                    (x.Name?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Amount.ToString().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Date.ShortFormat().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            IncomesCounter = filteredIncomes.Count();
            StateHasChanged();
        }
    }
}