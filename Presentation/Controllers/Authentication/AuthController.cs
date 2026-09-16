using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Token;
using Artway.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artway.Presentation.Controllers.Authentication
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        // Token API should always be 'Post'
        [HttpPost("token")]
        public ActionResult<AuthTokenDto> Token()
        {
            var result = _authService.GetJWTToken();
            return Ok(result);
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
