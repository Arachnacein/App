using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class LogInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
