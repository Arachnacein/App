using Microsoft.AspNetCore.Authorization;

namespace IdentityManager.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("editUser")]
    public async Task<IActionResult> EditUser([FromBody] UserModel model, CancellationToken ct)
    {
        await _userService.UpdateUserPropertiesAsync(model, ct);
        return NoContent();
    }

    [HttpPut("enableUser")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> EnableUser([FromBody] UserModel model, CancellationToken ct)
    {
        await _userService.EnableDisableUserAsync(model, ct);
        return NoContent();
    }

    [HttpGet("getUserData")]
    public async Task<IActionResult> GetUserData([FromQuery] Guid userId, CancellationToken ct)
    {
        var result = await _userService.GetUserDataAsync(userId, ct);
        return Ok(result);
    }

    [HttpPost("verifyEmail")]
    public async Task<IActionResult> VerifyEmail([FromBody] Guid userId, CancellationToken ct)
    {
        await _userService.SendVerificationEmailAsync(userId, ct);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var result = await _userService.GetUsersAsync(ct);
        return Ok(result);
    }

    [HttpPut("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model, CancellationToken ct)
    {
        await _userService.ChangePasswordAsync(model, ct);
        return NoContent();
    }

    [HttpPost("roles/assign")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> AssignRole([FromQuery] Guid userId, [FromQuery] string role, CancellationToken ct)
    {
        await _userService.AssignRoleAsync(userId, role, ct);
        return NoContent();
    }

    [HttpDelete("roles/remove")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> RemoveRole([FromQuery] Guid userId, [FromQuery] string role, CancellationToken ct)
    {
        await _userService.RemoveRoleAsync(userId, role, ct);
        return NoContent();
    }
}
