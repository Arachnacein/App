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

        private Task Submit()
        {
            if (form.IsValid)
            {

            }
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