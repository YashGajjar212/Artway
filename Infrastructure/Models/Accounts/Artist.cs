using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Artway.Infrastructure.Models.Customers
{
    public class Artist
    {
        [Key]
        public int ArtistId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        [MaxLength(100)]
        public string DisplayName { get; set; }

        [Required]
        public int AccountId { get; set; }

        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        public DateTime Last_login { get; set; }

        // Creation_date
        // Last_Updated
    }
}