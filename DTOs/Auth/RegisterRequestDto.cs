using System.ComponentModel.DataAnnotations;

namespace Artway.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}