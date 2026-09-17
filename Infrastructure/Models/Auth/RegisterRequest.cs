using System.ComponentModel.DataAnnotations;

namespace Artway.Infrastructure.Models.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}