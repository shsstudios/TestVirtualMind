using Microsoft.EntityFrameworkCore;
using Test.Entities;

namespace Test.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Purchase> Purchases { get; set; }
    }
}

