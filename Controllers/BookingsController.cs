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
                availableDriver.IsAvailable = false; // Set to unavailable
            }
            else
            {
                var random = new Random();
                booking.Status = random.Next(0, 2) == 0 ? "Cancelled" : "Pending";
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
    }
}
