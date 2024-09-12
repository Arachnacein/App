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
        private DateTime CurrentDate;


        protected override async Task OnInitializedAsync()
        {
            CurrentDate = DateTime.Now;

            await base.OnInitializedAsync();
            await LoadTransactions();
        }
        private async Task LoadTransactions()
        {
            try
            {
                transactions = await httpClient.GetFromJsonAsync<List<TransactionViewModel>>("/api/budget");
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
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Small };

            var parameters = new DialogParameters();
            parameters["Refresh"] = new Func<Task>(LoadTransactions);

            dialogService.ShowAsync<AddTransactionDialog>("Add new transaction", parameters, options);
        }
        private async Task EditTransaction(TransactionViewModel model)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(LoadTransactions);

            dialogService.ShowAsync<EditTransactionDialog>("Edit transaction", parameters, options);
        } 
        private async Task DeleteTransaction(TransactionViewModel model)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true };
            var parameters = new DialogParameters();
            parameters[nameof(model)] = model;
            parameters["Refresh"] = new Func<Task>(LoadTransactions);

            dialogService.ShowAsync<DeleteTransactionDialog>("Delete transaction", parameters, options);
        }
        private async Task ItemUpdated(MudItemDropInfo<TransactionViewModel> dropItem)
        {
            //parses string into enum
            dropItem.Item.Category = (TransactionCategoryEnum)Enum.Parse(typeof(TransactionCategoryEnum), dropItem.DropzoneIdentifier);

            await httpClient.PutAsJsonAsync<UpdateTransactionCategoryViewModel>("/api/budget/UpdateCategory", new UpdateTransactionCategoryViewModel {Id = dropItem.Item.Id, Category = dropItem.Item.Category });
        }

        private async Task PreviousMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            await LoadTransactions();
        }
        private async Task NextMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            await LoadTransactions();
        }
    }
}
