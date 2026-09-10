using System.ComponentModel.DataAnnotations;

namespace Artway.DTOs.Customers
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string Email { get; set; }

        //[Required]
        //public int AddressId { get; set; }

        //public 

        public string PasswordHash { get; set; }

        public int UserRole { get; set; } = 1;

        public DateTime Creation_Date { get; set; }

        public DateTime? Last_Updated { get; set; }

        public DateTime? Last_Login { get; set; }
    }
}