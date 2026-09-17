namespace Artway.Presentation.DTOs.Auth
{
    public class RegisterResponseDto
    {
        public int AccountId { get; set; }

        public string Email { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Token { get; set; }
    }
}