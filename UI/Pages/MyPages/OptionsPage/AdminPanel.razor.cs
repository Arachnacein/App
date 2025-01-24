using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class AdminPanel
    {
        [Inject] IStringLocalizer<AdminPanel> Localizer { get; set; }
    }
}