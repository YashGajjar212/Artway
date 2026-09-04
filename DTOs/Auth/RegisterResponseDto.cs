namespace Artway.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public int CustomerId { get; set; }

        public string Email { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Token { get; set; }
    }
}