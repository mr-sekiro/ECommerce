using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private string GetUserEmail()
        {
            return User.FindFirstValue(ClaimTypes.Email);
        }

        public AuthController(IServiceManager serviceManager)
        {
            _authService = serviceManager.AuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (result == null)
                return Conflict(new { Message = $"User with email {dto.Email} already exists." });

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { Message = "Invalid email or password." });

            return Ok(result);
        }

        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            var exists = await _authService.CheckEmail(email);
            return Ok(new { Exists = exists });
        }

        [Authorize]
        [HttpGet("current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var email = GetUserEmail();
            var result = await _authService.GetCurrentUser(email);
            if (result == null)
                return NotFound(new { Message = "User not found." });

            return Ok(result);
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            var email = GetUserEmail();

            var result = await _authService.GetUserAddressAsync(email);
            if (result == null)
                return NotFound(new { Message = "No address found for this user." });

            return Ok(result);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<IActionResult> UpdateCurrentUserAddress([FromBody] AddressDto dto)
        {
            var email = GetUserEmail();

            var updated = await _authService.UpdateUserAddressAsync(email, dto);

            if (updated == null)
                return BadRequest(new { Message = "Failed to update user address." });

            return Ok(new
            {
                Message = "Address updated successfully.",
                Address = updated
            });
        }
    }
}
