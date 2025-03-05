using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages
{
    public partial class RecurringTransaction
    {
        [Inject] private IStringLocalizer<RecurringTransaction> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private List<RecurringTransactionViewModel> recurringTransactions = new List<RecurringTransactionViewModel>();
        private int RecurringTransactionsCounter { get; set; } = 0;
        protected override async Task OnInitializedAsync()
        {
            await LoadRecurringTransactions();
        }

        private async Task LoadRecurringTransactions()
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
                return;
            recurringTransactions = await httpClient.GetFromJsonAsync<List<RecurringTransactionViewModel>>($"/api/recurringTransaction/{UserSessionService.UserId}");
            StateHasChanged();
        }
        private async Task DeleteRecurringTransaction(RecurringTransactionViewModel model)
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
                return;
            var response = await httpClient.DeleteAsync($"/api/recurringTransaction/{model.Id}/user/{UserSessionService.UserId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadRecurringTransactions();
                Snackbar.Add("Pomyślnie usunieto transakcję", Severity.Success);
            }
            else
                Snackbar.Add("Coś poszło nie tak", Severity.Error);
        }
    }
}