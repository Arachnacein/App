using Microsoft.AspNetCore.Components;
using MudBlazor;
using UI.Models.ViewModels;

namespace UI.Components.Dialogs
{
    public partial class EditUserPropertiesDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public string Property { get; set; }
        [Inject] private UserDetailsViewModelValidator UserDetailsValidator { get; set; }
        private UserDetailsViewModel DialogModel = new UserDetailsViewModel();
        private MudForm Form;

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        private async Task Submit()
        {
            await Form.Validate();
            if (!Form.IsValid)
                return;
            if (UserSessionService == null || UserSessionService.UserId == Guid.Empty)
            {
                Snackbar.Add("", Severity.Warning);
                return;
            }
        }
        private async Task Cancel() => MudDialog.Cancel();
    }
}