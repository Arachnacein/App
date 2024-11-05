using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace UI.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        private readonly ILogger<ErrorModel> _logger;
        private readonly IStringLocalizer<ErrorModel> _localizer;

        public ErrorModel(ILogger<ErrorModel> logger, IStringLocalizer<ErrorModel> localizer)
        {
            _logger = logger;
            _localizer = localizer;
        }

        public string Title => _localizer["Title"];
        public string Error => _localizer["Error"];
        public string ErrorContent => _localizer["ErrorContent"];


        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}
