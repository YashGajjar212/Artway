namespace Artway.Infrastructure.Models.Auth
{
    public class LoginResponse
    {
        public string Email { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; }
    }
}
