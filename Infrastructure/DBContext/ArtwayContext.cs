using Artway.Infrastructure.Models.Customers;
using Microsoft.EntityFrameworkCore;

namespace Artway.Database.DBContext
{
    public class ArtwayContext : DbContext
    {
        public ArtwayContext(DbContextOptions<ArtwayContext> options) : base(options)
        {

        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Artist> Artists { get; set; }
    }
}