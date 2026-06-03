namespace IdentityManager.Controllers;

[ApiController]
[Route("api/login")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<IActionResult> Login([FromForm] LoginModel model, CancellationToken ct)
    {
        var (accessToken, refreshToken) = await _authService.LoginAsync(model.Username, model.Password, ct);
        return Ok(new { access_token = accessToken, refresh_token = refreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenModel model, CancellationToken ct)
    {
        var (accessToken, refreshToken) = await _authService.RefreshTokenAsync(model.Token, ct);
        return Ok(new { access_token = accessToken, refresh_token = refreshToken });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenModel model, CancellationToken ct)
    {
        await _authService.LogoutAsync(model.Token, ct);
        return NoContent();
    }
}
