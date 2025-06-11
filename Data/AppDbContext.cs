using Microsoft.EntityFrameworkCore;
using MyTaxiService.Models;



namespace MyTaxiService.Data
{
    public class AppDbContext : DbContext
    {
    public AppDbContext(DbContextOptions<AppDbContext> options) : 
            base(options) { }


        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<CancellationLog> CancellationLogs { get; set; }
    
    
    }


}
