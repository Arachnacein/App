using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages
{
    public partial class AdminPanelUsersPage
    {
        [Inject] private IStringLocalizer<AdminPanel> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }

        private List<UserDetailsViewModel> users = new();

        protected override async Task OnInitializedAsync()
        {
            if (UserSessionService != null && UserSessionService.IsAdmin)
            {
                users = await FetchUsersAsync();
            }
        }

        private async Task<List<UserDetailsViewModel>> FetchUsersAsync()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<List<UserDetailsViewModel>>("api/User");
                foreach (var item in response)
                {
                    Console.WriteLine("Enabled: " + item.Enabled);
                    Console.WriteLine("EmailVerified: " + item.EmailVerified);
                }

                return response ?? new List<UserDetailsViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception(Localizer["Failed to fetch users."], ex);
            }
        }
    }
}