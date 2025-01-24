using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages
{
    public partial class AdminPanelHelathChechPage
    {
        [Inject] IStringLocalizer<AdminPanel> Localizer { get; set; }
    }
}