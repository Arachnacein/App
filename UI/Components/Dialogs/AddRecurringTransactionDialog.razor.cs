using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Inject] IDialogService DialogService { get; set; }
        private MudForm form;
        private RecurringTransaction Model;

        protected override Task OnInitializedAsync()
        {
            Model = new RecurringTransaction
            {
                Id = 1,
                UserId = Guid.NewGuid(),
                Name = "Sample Transaction",
                Description = "This is a sample recurring transaction",
                Amount = 100.0,
                TransactionType = TransactionTypeEnum.Expense,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(1),
                Approved = true,
                Frequency = FrequencyEnum.Monthly,
                Interval = 1,
                WeeklyDays = new List<DayOfWeek>(),
                MonthlyDay = 15,
                YearlyMonth = null,
                YearlyDay = null,
                MaxOccurrences = 12
            };

            return base.OnInitializedAsync();
        }

        private Task Submit()
        {
            //if (form.IsValid)
            //{

            //}
            return Task.CompletedTask;

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