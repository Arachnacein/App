using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogInController : ControllerBase
    {
        private readonly ILoginservice _loginservice;

        public LogInController(ILoginservice loginservice)
        {
            _loginservice = loginservice;
        }

        [HttpPost]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> LogIn([FromForm] LoginModel loginModel)
        {
            var token = await _loginservice.Login(loginModel.Username, loginModel.Password);

            if (token != null)
            {
                return Ok(new { access_token = token });
            }

            return BadRequest("Logowanie nie powiodło się.");
        }
    }
}