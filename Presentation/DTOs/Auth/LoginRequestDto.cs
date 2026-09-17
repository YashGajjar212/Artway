using System.Text.Json.Serialization;

namespace Artway.Presentation.DTOs.Auth
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}