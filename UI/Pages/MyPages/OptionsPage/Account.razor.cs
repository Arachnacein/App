using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Account
    {
        [Inject] private IStringLocalizer<Account> Localizer {  get; set; }
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
    }
}