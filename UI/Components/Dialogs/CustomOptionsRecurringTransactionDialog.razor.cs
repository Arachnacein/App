using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class CustomOptionsRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Parameter] public RecurringTransactionViewModel Model { get; set; }
        [Inject] private IStringLocalizer<CustomOptionsRecurringTransactionDialog> Localizer { get; set; }
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

        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
