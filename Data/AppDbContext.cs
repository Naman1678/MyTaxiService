using Microsoft.EntityFrameworkCore;
using MyTaxiService.Models;

namespace MyTaxiService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Ride> Rides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Booking -> User (Many-to-One)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking -> Driver (Many-to-One)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Driver)
                .WithMany(d => d.Bookings)
                .HasForeignKey(b => b.DriverId)
                .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}
