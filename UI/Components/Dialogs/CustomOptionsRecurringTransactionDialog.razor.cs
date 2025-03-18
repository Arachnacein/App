using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Dynamic;
using System.Net;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class CustomOptionsRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Parameter] public RecurringTransactionViewModel Model { get; set; }
        [Parameter] public Func<Task> Refresh { get; set; }
        [Inject] private IStringLocalizer<CustomOptionsRecurringTransactionDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        private bool CheckBoxMonday { get; set; } = false;
        private bool CheckBoxTuesday { get; set; } = false;
        private bool CheckBoxWednesday { get; set; } = false;
        private bool CheckBoxThursday { get; set; } = false;
        private bool CheckBoxFriday { get; set; } = false;
        private bool CheckBoxSaturday { get; set; } = false;
        private bool CheckBoxSunday { get; set; } = false;
        private string EndOption { get; set; } = "date";
        protected override Task OnInitializedAsync()
        {
            Model.Frequency = FrequencyEnum.Weekly;
            Model.MonthlyDay = Model.StartDate.Value.Day;
            Model.YearlyDay = Model.StartDate.Value.Day;
            Model.YearlyMonth = Model.StartDate.Value.Month;
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
                return;
            }

            if (EndOption == "date" && Model.EndDate < Model.StartDate)
            {
                Snackbar.Add(Localizer["EndDateGreaterThanStartDate"], Severity.Warning);
                return;
            }
            if (Model.MaxOccurrences < 1)
            {
                Snackbar.Add(Localizer["IntervalCorrectValue"], Severity.Warning);
                return;
            }

            if(EndOption == "date")
                Model.MaxOccurrences = 0;

            if(Model.Frequency == FrequencyEnum.Weekly)
                GetWeeklyDays();

            var result = await httpClient
                        .PostAsJsonAsync<RecurringTransactionViewModel>("/api/recurringTransaction/Custom", Model);

            if (result.StatusCode == HttpStatusCode.Created)
            {
                Snackbar.Add(Localizer["SuccessfullyAddRecurringTransaction"], Severity.Success);
                MudDialog.Cancel();
                if (Refresh != null)
                    await Refresh.Invoke();
            }
            else
                Snackbar.Add(Localizer["ErrorAddRecurringTransaction"], Severity.Error);
        }

        private void GetWeeklyDays()
        {
            Model.WeeklyDays.Clear();

            if (CheckBoxMonday)
                Model.WeeklyDays.Add(DayOfWeek.Monday);
            if (CheckBoxTuesday)
                Model.WeeklyDays.Add(DayOfWeek.Tuesday);
            if (CheckBoxWednesday)
                Model.WeeklyDays.Add(DayOfWeek.Wednesday);
            if (CheckBoxThursday)
                Model.WeeklyDays.Add(DayOfWeek.Thursday);
            if (CheckBoxFriday)
                Model.WeeklyDays.Add(DayOfWeek.Friday);
            if (CheckBoxSaturday)
                Model.WeeklyDays.Add(DayOfWeek.Saturday);
            if (CheckBoxSunday)
                Model.WeeklyDays.Add(DayOfWeek.Sunday);
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}