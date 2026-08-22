using Artway.Models;
using Microsoft.EntityFrameworkCore;

namespace Artway.Database.DBContext
{
    public class ArtwayContext : DbContext
    {
        public ArtwayContext(DbContextOptions<ArtwayContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}