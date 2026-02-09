using Microsoft.EntityFrameworkCore;
using capstone_prjct.Entities;

namespace capstone_prjct.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Policy> Policy { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserPolicy> UserPolicy { get; set; }
    }
}