using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTaxiService.Data;
using MyTaxiService.Models;

namespace MyTaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

     
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            booking.RequestedTime = DateTime.Now;

            var availableDriver = _context.Drivers
                .Where(d => d.IsAvailable)
                .FirstOrDefault();

            if (availableDriver != null)
            {
                booking.DriverId = availableDriver.DriverId;
                booking.Status = "Accepted";
                availableDriver.IsAvailable = false;
            }
            else
            {
                booking.Status = "Pending";
            }

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _context.Bookings
                .Include(b => b.Driver)
                .FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

       
        [HttpGet("pending")]
        [Authorize(Roles = "Driver")]
        public IActionResult GetPendingBookings()
        {
            var pending = _context.Bookings
                .Where(b => b.Status == "Pending")
                .ToList();

            return Ok(pending);
        }

       
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Driver")]
        public IActionResult AcceptBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking == null || booking.Status != "Pending")
                return NotFound("Booking not found or not pending.");

            booking.Status = "Accepted";

       
            if (booking.DriverId.HasValue)
            {
                var driver = _context.Drivers.Find(booking.DriverId.Value);
                if (driver != null)
                    driver.IsAvailable = false;
            }

            _context.SaveChanges();
            return Ok("Booking accepted.");
        }

       
        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Driver")]
        public IActionResult DeclineBooking(int id)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == id);
            if (booking == null || booking.Status != "Pending")
                return NotFound("Booking not found or not pending.");

            booking.Status = "Cancelled";

            _context.SaveChanges();
            return Ok("Booking declined.");
        }
    }
}
