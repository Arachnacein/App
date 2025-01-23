using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace UI.Components.Dialogs
{
    public partial class VerifyEmailDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public EventCallback OnDialogClose { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private async Task Send()
        {

        }
    }
}