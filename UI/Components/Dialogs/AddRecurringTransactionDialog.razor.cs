using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        private MudForm form;
        private RecurringTransaction Model;
        private bool ShowWeeklyDays = false;
        private bool ShowMonthlyDay = false;
        private bool ShowYearly = false;

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
                ScheduleId = 1,
                Schedule = new RecurringTransactionSchedule
                {
                    Id = 1,
                    Frequency = FrequencyEnum.Monthly,
                    Interval = 1,
                    WeeklyDays = new List<DayOfWeek>(),
                    MonthlyDay = 15,
                    YearlyMonth = null,
                    YearlyDay = null,
                    MaxOccurrences = 12,
                    RecurringTransactionId = 1
                }
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
        private void OnFrequencyChange(ChangeEventArgs e)
        {
            var selectedValue = e.Value?.ToString();

            ShowWeeklyDays = selectedValue == "FrequencyEnum.Weekly";
            ShowMonthlyDay = selectedValue == "FrequencyEnum.Monthly";
            ShowYearly = selectedValue == "FrequencyEnum.Yearly";
        }

        private void ToggleDay(DayOfWeek day)
        {
            if (Model.Schedule.WeeklyDays.Contains(day))
                Model.Schedule.WeeklyDays.Remove(day);
            else
                Model.Schedule.WeeklyDays.Add(day);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}