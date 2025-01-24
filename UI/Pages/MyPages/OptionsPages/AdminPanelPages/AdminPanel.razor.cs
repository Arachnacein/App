using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace UI.Pages.MyPages.OptionsPages.AdminPanelPages
{
    public partial class AdminPanel
    {
        [Inject] IStringLocalizer<AdminPanel> Localizer { get; set; }
    }
}