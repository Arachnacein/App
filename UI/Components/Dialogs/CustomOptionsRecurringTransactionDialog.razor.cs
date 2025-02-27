using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class CustomOptionsRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }
        [Parameter] public RecurringTransaction Model { get; set; }
        private bool CheckBoxMonday { get; set; } = false;
        private bool CheckBoxTuesday { get; set; } = false;
        private bool CheckBoxWednesday { get; set; } = false;
        private bool CheckBoxThursday { get; set; } = false;
        private bool CheckBoxFriday { get; set; } = false;
        private bool CheckBoxSaturday { get; set; } = false;
        private bool CheckBoxSunday { get; set; } = false;

        protected override Task OnInitializedAsync()
        {
            Model.Frequency = FrequencyEnum.Daily;
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {

        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
