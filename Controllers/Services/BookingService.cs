using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyTaxiService.Data;
using MyTaxiService.Hubs;
using MyTaxiService.Models;

namespace MyTaxiService.Controllers.Services
{
    public class BookingService(AppDbContext ctx, IHubContext<RideHub> hub)
    {
        private readonly AppDbContext _context = ctx;
        private readonly IHubContext<RideHub> _hub = hub;

        public Booking CreateBooking(Booking b)
        {
            b.RequestedTime = DateTime.Now;
            b.Status = "Pending";
            _context.Bookings.Add(b);
            _context.SaveChanges();
            return b;
        }

        public async Task<Booking?> AcceptBooking(int bookingId, int driverId)
        {
            var booking = _context.Bookings.FirstOrDefault(
                              b => b.BookingId == bookingId && b.Status == "Pending");
            var driver = _context.Drivers.Find(driverId);
            if (booking == null || driver == null || !driver.IsAvailable) return null;

            booking.Status = "Accepted";
            booking.DriverId = driverId;
            driver.IsAvailable = false;
            _context.SaveChanges();

            // Send update to clients
            await _hub.Clients.All.SendAsync("RideUpdated", booking);
            return booking;
        }

        public async Task<Booking?> DeclineBooking(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(
                              b => b.BookingId == bookingId && b.Status == "Pending");
            if (booking == null) return null;

            booking.Status = "Cancelled";
            _context.SaveChanges();

            // Notify all clients about the declined ride
            await _hub.Clients.All.SendAsync("RideUpdated", booking);
            return booking;
        }

        public List<Booking> GetPendingBookings() =>
            [.. _context.Bookings.Where(b => b.Status == "Pending")];

        public Booking? GetBookingById(int id) =>
            _context.Bookings.Include(b => b.Driver)
                             .FirstOrDefault(b => b.BookingId == id);

        public Booking? GetLatestBookingByUser(int userId) =>
            _context.Bookings.Where(b => b.UserId == userId)
                             .OrderByDescending(b => b.RequestedTime)
                             .FirstOrDefault();
    }
}
