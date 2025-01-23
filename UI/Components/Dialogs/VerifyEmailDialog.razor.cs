using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Text.Json;
using System.Text;

namespace UI.Components.Dialogs
{
    public partial class VerifyEmailDialog
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [Parameter] public EventCallback OnDialogClose { get; set; }
        [Inject] private IStringLocalizer<VerifyEmailDialog> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        private async Task Send()
        {
            var requestBody = new StringContent(
                JsonSerializer.Serialize(UserSessionService.UserId),
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync($"/api/User/verifyEmail", requestBody);
            if(response.IsSuccessStatusCode)
                Snackbar.Add(Localizer["EmailSent"], Severity.Success);
            else
                Snackbar.Add(Localizer["EmailNotSent"], Severity.Warning);
            //update user session service.verifyemail 
            //wait 2 seconds
            //cancel dialog
        }
    }
}