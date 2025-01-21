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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("editUser")]
        public async Task<IActionResult> EditUser([FromBody] UserModel model)
        {
            await _userService.UpdateUserPropertiesAsync(model);
            return NoContent();
        }

        [HttpGet("getUserData")]
        public async Task<IActionResult> GetUserData([FromQuery] Guid userId)
        {
            var result = await _userService.GetUserDataAsync(userId);
            return Ok(result);
        }
    }
}