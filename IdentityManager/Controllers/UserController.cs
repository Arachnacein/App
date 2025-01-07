using IdentityManager.Models;
using IdentityManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenSetvice)
        {
            _userService = userService;
            _tokenService = tokenSetvice;
        }

        [HttpPut("editUser")]
        public async Task<IActionResult> EditUser([FromBody] UserModel model)
        {
            await _userService.UpdateUserPropertiesAsync(model);
            return NoContent();
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromQuery] string token)
        {
            var result = await _tokenService.RefreshAccessTokenAsync(token);
            if (token != null)
            {
                return Ok(new { access_token = token });
            }

            return BadRequest("Błąd podczas odświeżania tokenu");
        }
    }
}