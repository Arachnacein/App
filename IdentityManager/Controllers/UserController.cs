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

        [HttpGet("getUserData")]
        public async Task<IActionResult> GetUserData([FromQuery] Guid userId)
        {
            Console.WriteLine("_____________ Jestem w kontrolerze getUserData");
            var result = await _userService.GetUserDataAsync(userId);
            Console.WriteLine("_____________ Znów wróciłem do kontrolera");
            return Ok(result);
        }
    }
}