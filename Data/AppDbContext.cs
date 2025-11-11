using HealthBridgeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthBridgeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {
        }
        public DbSet<User> Users { get; set; } = null;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(); ;
            base.OnModelCreating(modelBuilder);
        }
    }
}
