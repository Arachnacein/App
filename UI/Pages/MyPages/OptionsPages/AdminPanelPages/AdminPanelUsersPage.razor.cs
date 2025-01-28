using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using UI.Extensions;
using UI.Models.ViewModels;

namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages
{
    public partial class AdminPanelUsersPage
    {
        [Inject] private IStringLocalizer<AdminPanel> Localizer { get; set; }
        [Inject] private HttpClient httpClient { get; set; }

        private List<UserDetailsViewModel> users = new List<UserDetailsViewModel>();
        private List<UserDetailsViewModel> filteredUsers = new List<UserDetailsViewModel>();
        private string _searchPhrase = string.Empty;
        public string searchPhrase // dynamic filtering
        {
            get => _searchPhrase;
            set
            {
                if (_searchPhrase != value)
                {
                    _searchPhrase = value;
                    FilterUsers();
                }
            }
        }
        public int usersCounter { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if (UserSessionService != null && UserSessionService.IsAdmin)
                await FetchUsersAsync();
        }

        private async Task FetchUsersAsync()
        {
            users = await httpClient.GetFromJsonAsync<List<UserDetailsViewModel>>("api/User");
            filteredUsers = users;
            StateHasChanged();
        }
        private void FilterUsers()
        {
            if (string.IsNullOrWhiteSpace(searchPhrase) || searchPhrase.Length < 3)
                filteredUsers = users;
            else
            {
                filteredUsers = users.Where(x =>
                    (x.FirstName?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.LastName?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.Email?.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (x.AccountCreatedDate.ShortFormat().Contains(searchPhrase, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }
            usersCounter = filteredUsers.Count();
            StateHasChanged();
        }
        private async Task Enable(UserDetailsViewModel model)
        {
            model.Enabled = !model.Enabled;
            await httpClient.PutAsJsonAsync("api/User/enableUser", model);
        }
    }
}