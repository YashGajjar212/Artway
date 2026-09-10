using System.ComponentModel.DataAnnotations;

namespace Artway.Models.Auth
{
    public class RegisterRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}