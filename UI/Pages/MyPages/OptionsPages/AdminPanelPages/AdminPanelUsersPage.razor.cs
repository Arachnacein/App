using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages
{
    public partial class AdminPanelUsersPage
    {
        [Inject] IStringLocalizer<AdminPanel> Localizer { get; set; }
    }
}