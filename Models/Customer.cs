using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artway.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; }

        //[Required]
        //public int AddressId { get; set; }

        //public 

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public int UserRole { get; set; }

        [Required]
        public DateTime Creation_Date { get; set; }

        [Required]
        public DateTime Last_Updated { get; set; }

        [Required]
        public DateTime Last_Login { get; set; }
    }
}