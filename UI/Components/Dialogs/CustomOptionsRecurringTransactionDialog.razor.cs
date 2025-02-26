
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace UI.Components.Dialogs
{
    public partial class CustomOptionsRecurringTransactionDialog
    {
        [CascadingParameter] public MudDialogInstance MudDialog { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {

        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}
