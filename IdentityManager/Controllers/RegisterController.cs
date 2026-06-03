using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers;

[ApiController]
[Route("api/register")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Register([FromForm] RegistrationModel model, CancellationToken ct)
    {
        await _accountService.RegisterAsync(model, ct);
        return Ok();
    }
}
