using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

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
    }
}
