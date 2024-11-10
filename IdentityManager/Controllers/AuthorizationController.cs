using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
