using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Token;
using Artway.DTOs.Auth;
using Artway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Artway.Presentation.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> RegisterCustomer(RegisterRequestDto registerRequestDto)
        {
            var result = await _authService.RegisterCustomer(registerRequestDto);
            return Ok(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginRequest)
        {
            var result = await _authService.Login(loginRequest);
            var token = _tokenService.GenerateToken(loginRequest.Email);

            var response = new LoginResponseDto
            {
                Email = loginRequest.Email,
                ExpiresAt = DateTime.UtcNow.AddMinutes(20),
                Token = token
            };

            return Ok(response);
        }
    }
}
