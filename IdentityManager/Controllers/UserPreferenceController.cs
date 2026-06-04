namespace IdentityManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserPreferenceController : ControllerBase
{
    private readonly IUserPreferenceService _service;

    public UserPreferenceController(IUserPreferenceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string userId, CancellationToken ct)
    {
        var result = await _service.GetAsync(userId, ct);
        if (result is null)
            return NoContent();
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Save([FromBody] UserPreferenceModel model, CancellationToken ct)
    {
        await _service.UpsertAsync(model, ct);
        return NoContent();
    }
}
