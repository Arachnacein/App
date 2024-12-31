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
        private UserDetailsViewModel UserDetails = new UserDetailsViewModel();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataToUserDetailsModel();
        }

        private async Task LoadDataToUserDetailsModel()
        {
            UserDetails.UserId = UserSessionService.UserId;
            UserDetails.FirstName = UserSessionService.Name;
            UserDetails.LastName = UserSessionService.Surname;
            UserDetails.Username = UserSessionService.Username;
            UserDetails.Email = UserSessionService.Email;
            UserDetails.Roles = UserSessionService.Roles
                                    .Where(role => role.Contains("user") || role.Contains("admin"))
                                    .ToList() ?? new List<string>();
            UserDetails.AccountCreatedDate = UserSessionService.AccountCreatedDate;
            UserDetails.SessionExpiryDate = UserSessionService.TokenExpiryDate;

        }
        private async Task EditData(string variableName)
        {
            switch (variableName)
            {
                case nameof(UserDetails.FirstName):
                    await OpenDialog(nameof(UserDetails.FirstName));
                    break;                
                case nameof(UserDetails.LastName):
                    await OpenDialog(nameof(UserDetails.LastName));
                    break;                
                case nameof(UserDetails.Username):
                    await OpenDialog(nameof(UserDetails.Username));
                    break;                
                case nameof(UserDetails.Email):
                    await OpenDialog(nameof(UserDetails.Email));
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
            parameters[nameof(UserDetails)] = UserDetails;
            //refresh data send update token TODO 

            await dialogService.ShowAsync<EditUserPropertiesDialog>($"Edycja {property} uzytkownika", parameters, options);
        }
    }
}