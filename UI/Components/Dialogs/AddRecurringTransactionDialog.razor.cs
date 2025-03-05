using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Net;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class AddRecurringTransactionDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Inject] private IDialogService DialogService { get; set; }
        [Inject] private IStringLocalizer<AddRecurringTransactionDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }
        [Inject] private RecurringTransactionViewModelValidator RecurringTransactionValidator { get; set; }
        private MudForm form;
        private RecurringTransactionViewModel Model;

        protected override async Task OnInitializedAsync()
        {
            Model = new RecurringTransactionViewModel
            {
                UserId = UserSessionService.UserId,
                Name = String.Empty,
                Description = String.Empty,
                TransactionType = TransactionTypeEnum.Expense,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(1),
                Frequency = FrequencyEnum.Monthly,
                Interval = 1,
                WeeklyDays = new List<DayOfWeek>(),
                MaxOccurrences = 12
            };
        }

        private async Task Submit()
        {
            await form.Validate();
            if (!form.IsValid)
                return;

            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add(Localizer["MustSignIn"], Severity.Warning);
                return;
            }
            
            var result = await httpClient.PostAsJsonAsync<RecurringTransactionViewModel>("/api/recurringTransaction", Model);
            if(result.StatusCode == HttpStatusCode.Created)
            {
                Snackbar.Add(Localizer["SuccessfullyAddRecurringTransaction"], Severity.Success);
                MudDialog.Cancel();    
            }
            else
                Snackbar.Add(Localizer["ErrorAddRecurringTransaction"], Severity.Error);

        }

        private async Task OpenCustomDialog()
        {
            if (Model.Frequency == FrequencyEnum.Custom)
            {
                var parameters = new DialogParameters();
                parameters["Model"] = Model;

                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small };
                await DialogService.ShowAsync<CustomOptionsRecurringTransactionDialog>(Localizer["CustomOptions"], parameters, options);
            }
        }

        private async Task Cancel() => MudDialog.Cancel();
    }
}