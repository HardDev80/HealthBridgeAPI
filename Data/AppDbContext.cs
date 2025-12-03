using HealthBridgeAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthBridgeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Practitioner> Practitioners { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<AvailableDoctor> AvailableDoctors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique(); ;
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Location>().Property(p => p.LocationId).ValueGeneratedOnAdd();
            modelBuilder.Entity<AvailableDoctor>().ToTable("AvailableDoctorsCache").HasKey(x => x.PractitionerId);
        }
    }
}
