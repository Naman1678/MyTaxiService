using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTaxiService.Models;
using MyTaxiService.Services;

namespace MyTaxiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController(BookingService bookingService) : ControllerBase
    {
        private readonly BookingService _bookingService = bookingService;

        [HttpPost]
        [Authorize]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            var createdBooking = _bookingService.CreateBooking(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = createdBooking.BookingId }, createdBooking);
        }

        [HttpPut("{id}/accept")]
        [Authorize]
        public async Task<IActionResult> AcceptBooking(int id, [FromQuery] int driverId)
        {
            var result = await _bookingService.AcceptBooking(id, driverId);
            if (result == null)
                return BadRequest("Booking not found or driver unavailable.");

            return Ok(result);
        }

        [HttpPut("{id}/decline")]
        [Authorize]
        public async Task<IActionResult> DeclineBooking(int id)
        {
            var result = await _bookingService.DeclineBooking(id);
            if (result == null)
                return BadRequest("Booking not found or already processed.");

            return Ok(result);
        }

        [HttpGet("pending")]
        [Authorize]
        public IActionResult GetPendingBookings()
        {
            var bookings = _bookingService.GetPendingBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetBookingById(int id)
        {
            var booking = _bookingService.GetBookingById(id);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }

        [HttpGet("client-latest")]
        [Authorize]
        public IActionResult GetLatestBookingByUser([FromQuery] int userId)
        {
            var booking = _bookingService.GetLatestBookingByUser(userId);
            if (booking == null)
                return NotFound();

            return Ok(booking);
        }
    }
}
