﻿using IdentityManager.Models;
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

        [HttpPut("enableUser")]
        public async Task<IActionResult> EnableUser([FromBody] UserModel model)
        {
            await _userService.EnableDisableUserAsync(model);
            return NoContent();
        }

        [HttpGet("getUserData")]
        public async Task<IActionResult> GetUserData([FromQuery] Guid userId)
        {
            var result = await _userService.GetUserDataAsync(userId);
            return Ok(result);
        }

        [HttpPost("verifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] Guid userId)
        {
            await _userService.SendVerificationEmailAsync(userId);
            return NoContent();
        }        
        
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.GetUsersAsync();
            return Ok(result);
        }
    }
}