using Artway.Presentation.DTOs.Auth;

namespace Artway.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        AuthTokenDto GetJWTToken();

        Task<RegisterResponseDto> RegisterAccount(RegisterRequestDto registerRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}