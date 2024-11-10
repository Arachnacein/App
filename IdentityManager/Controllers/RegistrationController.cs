using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
