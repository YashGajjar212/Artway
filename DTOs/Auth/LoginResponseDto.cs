namespace Artway.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; }
    }
}