using Microsoft.AspNetCore.Components;
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

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            LoadTransactions();
        }
        private async Task LoadTransactions()
        {
            try
            {
                transactions = await httpClient.GetFromJsonAsync<List<TransactionViewModel>>("/api/budget");
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
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();

            dialogService.ShowAsync<AddTransactionDialog>("Add new transaction", parameters, options);
        }
        private async Task EditTransaction(TransactionViewModel model)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;

            dialogService.ShowAsync<EditTransactionDialog>("Edit transaction", parameters, options);
        } 
        private async Task DeleteTransaction(TransactionViewModel model)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;

            dialogService.ShowAsync<DeleteTransactionDialog>("Delete transaction", parameters, options);
        }
    }
}
