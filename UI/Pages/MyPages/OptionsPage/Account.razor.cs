using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;
using UI.Components.Dialogs;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Account
    {
        [Inject] private IStringLocalizer<Account> Localizer {  get; set; }
        [Inject] private IDialogService dialogService { get; set; }
        private UserDetailsViewModel userDetails = new UserDetailsViewModel();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataToUserDetailsModel();
        }

        private async Task LoadDataToUserDetailsModel()
        {
            userDetails.FirstName = UserSessionService.Name;
            userDetails.LastName = UserSessionService.Surname;
            userDetails.Username = UserSessionService.Username;
            userDetails.Email = UserSessionService.Email;
            userDetails.Roles = UserSessionService.Roles
                                    .Where(role => role.Contains("user") || role.Contains("admin"))
                                    .ToList() ?? new List<string>();
            userDetails.AccountCreatedDate = UserSessionService.AccountCreatedDate;
            userDetails.SessionExpiryDate = UserSessionService.TokenExpiryDate;

        }
        private async Task EditData(string variableName)
        {
            switch (variableName)
            {
                case nameof(userDetails.FirstName):
                    await OpenDialog(nameof(userDetails.FirstName));
                    break;                
                case nameof(userDetails.LastName):
                    await OpenDialog(nameof(userDetails.LastName));
                    break;                
                case nameof(userDetails.Username):
                    await OpenDialog(nameof(userDetails.Username));
                    break;                
                case nameof(userDetails.Email):
                    await OpenDialog(nameof(userDetails.Email));
                    break;                
                default:
                    break;
            }
        }
        private async Task OpenDialog(string property)
        {
            var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.ExtraSmall };

            var parameters = new DialogParameters();
            parameters[nameof(property)] = property;
            //refresh data send update token TODO 

            await dialogService.ShowAsync<EditUserPropertiesDialog>($"Edycja {property} uzytkownika", parameters, options);
        }
    }
}