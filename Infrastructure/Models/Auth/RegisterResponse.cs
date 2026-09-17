namespace Artway.Infrastructure.Models.Auth
{
    public class RegisterResponse
    {
        public int AccountId { get; set; }

        public string Email { get; set; }

        public DateTime ExpiresAt { get; set; }

        public string Token { get; set; }
    }
}