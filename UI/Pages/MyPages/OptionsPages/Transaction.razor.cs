using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Extensions;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages
{
    public partial class Transaction
    {
        [Inject] private IDialogService dialogService { get; set; }
        [Inject] private ISnackbar snackbar { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private List<TransactionViewModel> transactions = new List<TransactionViewModel>();
        private List<TransactionViewModel> filteredTransactions = new List<TransactionViewModel>();
        private string _searchPhrase = string.Empty;
        public string searchPhrase //This is for dynamic filtering
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
        private int transactionCounter { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            await LoadTransactions();
            transactionCounter = transactions.Count();
        }
        private async Task LoadTransactions()
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
                return;

            transactions = await httpClient.GetFromJsonAsync<List<TransactionViewModel>>($"/api/transaction?userId={UserSessionService.UserId}");
            filteredTransactions = transactions;
            StateHasChanged();
        }
        private void FilterTransactions()
        {
            if (string.IsNullOrWhiteSpace(searchPhrase) || searchPhrase.Length < 3)
                filteredTransactions = transactions;
            else
            {
                filteredTransactions = transactions.Where(x =>
                    (x.Name?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Description?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Category.ToString().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Price.ToString().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Date.FormatMY().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            transactionCounter = filteredTransactions.Count();
            StateHasChanged();
        }
        private async Task EditOptions(TransactionViewModel model)
        {
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(LoadTransactions);
            var options = new DialogOptions { CloseOnEscapeKey = true };

            dialogService.Show<EditDeleteTransactionDialog>(Localizer["Options"], parameters, options);

        }
    }
}