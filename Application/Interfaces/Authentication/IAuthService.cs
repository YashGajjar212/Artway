using Artway.DTOs.Auth;

namespace Artway.Application.Interfaces.Authentication
{
    public interface IAuthService
    {
        AuthTokenDto GetJWTToken();

        Task<RegisterResponseDto> RegisterCustomer(RegisterRequestDto registerRequestDto);

        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}