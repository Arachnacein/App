using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace UI.Pages.MyPages.OptionsPage
{
    public partial class Account
    {
        [Inject] private IStringLocalizer<Account> Localizer {  get; set; }

    }
}