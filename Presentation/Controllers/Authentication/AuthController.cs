using Artway.Application.Interfaces.Authentication;
using Artway.DTOs.Auth;
using Artway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Artway.Presentation.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> RegisterCustomer(RegisterRequestDto registerRequestDto)
        {
            var result = await _authService.RegisterCustomer(registerRequestDto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var result = await _authService.Login(loginRequest);
            return Ok(result);
        }
    }
}
