using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private MudForm form;
        private RecurringTransactionViewModel Model;

        protected override Task OnInitializedAsync()
        {
            Model = new RecurringTransactionViewModel
            {
                UserId = UserSessionService.UserId,
                Name = String.Empty,
                Description = String.Empty,
                TransactionType = TransactionTypeEnum.Expense,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Approved = false,
                Frequency = FrequencyEnum.Monthly,
                Interval = 1,
                WeeklyDays = new List<DayOfWeek>(),
                MaxOccurrences = 12
            };

            return base.OnInitializedAsync();
        }

        private async Task Submit()
        {
            if (form.IsValid)
            {
                var result = await httpClient.PostAsJsonAsync<RecurringTransactionViewModel>("/api/recurringTransaction", Model);
                if(result.IsSuccessStatusCode)
                    Snackbar.Add("Pomyślnie dodano transakcję cykliczną", Severity.Success);
                else
                    Snackbar.Add("Wystąpił błąd podczas dodawania transakcji cyklicznej", Severity.Error);
            }

        }

        private async Task OpenCustomDialog()
        {
            if (Model.Frequency == FrequencyEnum.Custom)
            {
                var parameters = new DialogParameters();
                parameters["Model"] = Model;

                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
                await DialogService.ShowAsync<CustomOptionsRecurringTransactionDialog>("Ustawienia niestandardowe", parameters, options);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}