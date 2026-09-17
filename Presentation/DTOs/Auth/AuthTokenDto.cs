namespace Artway.Presentation.DTOs.Auth
{
    public class AuthTokenDto
    {
        public string Issuer { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public int ExpireyInSeconds { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}