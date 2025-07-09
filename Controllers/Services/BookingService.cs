using Microsoft.EntityFrameworkCore;
using MyTaxiService.Data;
using MyTaxiService.Models;

namespace MyTaxiService.Controllers.Services
{
    public class BookingService(AppDbContext context)
    {
        private readonly AppDbContext _context = context;

        public Booking CreateBooking(Booking booking)
        {
            booking.RequestedTime = DateTime.Now;
            booking.Status = "Pending";
            booking.DriverId = null;

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return booking;
        }

        public Booking ? AcceptBooking(int bookingId, int driverId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.Status == "Pending");
            if (booking == null) return null;

            var driver = _context.Drivers.Find(driverId);
            if (driver == null || !driver.IsAvailable) return null;

            booking.Status = "Accepted";
            booking.DriverId = driverId;
            driver.IsAvailable = false;

            _context.SaveChanges();
            return booking;
        }

        public Booking ? DeclineBooking(int bookingId)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId && b.Status == "Pending");
            if (booking == null) return null;

            booking.Status = "Cancelled";
            _context.SaveChanges();
            return booking;
        }


        public List<Booking> GetPendingBookings()
        {
            return [.. _context.Bookings.Where(b => b.Status == "Pending")];
        }

        public Booking GetBookingById(int id)
        {
            return _context.Bookings
                .Include(b => b.Driver)
                .FirstOrDefault(b => b.BookingId == id);
        }
    }
}
