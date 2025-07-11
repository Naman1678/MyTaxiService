using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyTaxiService.Controllers.Services;
using MyTaxiService.Data;
using MyTaxiService.Models;

namespace MyTaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly BookingService _service;

        public BookingsController(AppDbContext context)
        {
            _service = new BookingService(context);
        }

        /// <summary>
        /// Creates a new ride booking. The default status is set to "Pending".
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Client")]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = _service.CreateBooking(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = created.BookingId }, created);
        }

        /// <summary>
        /// Fetch a specific booking by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _service.GetBookingById(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        /// <summary>
        /// Returns all bookings that are still pending.
        /// </summary>
        [HttpGet("pending")]
        [Authorize(Roles = "Driver")]
        public IActionResult GetPendingBookings()
        {
            var pending = _service.GetPendingBookings();
            return Ok(pending);
        }

        /// <summary>
        /// Accepts a pending booking and assigns it to a driver.
        /// </summary>
        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Driver")]
        public IActionResult AcceptBooking(int id, [FromQuery] int driverId)
        {
            var updated = _service.AcceptBooking(id, driverId);
            if (updated == null) return BadRequest("Booking cannot be accepted.");
            return Ok("Booking accepted.");
        }

        /// <summary>
        /// Declines a pending booking and marks it as "Cancelled".
        /// </summary>
        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Driver")]
        public IActionResult DeclineBooking(int id)
        {
            var updated = _service.DeclineBooking(id);
            if (updated == null) return BadRequest("Booking cannot be declined.");
            return Ok("Booking declined.");
        }

        /// <summary>
        /// Returns the most recent booking for a client by user ID.
        /// </summary>
        [HttpGet("client-latest")]
        [Authorize(Roles = "Client")]
        public IActionResult GetLatestBooking([FromQuery] int userId)
        {
            var booking = _service.GetLatestBookingByUser(userId);
            if (booking == null)
                return NotFound("No booking found.");

            return Ok(new
            {
                booking.BookingId,
                booking.Status,
                booking.PickupLocation,
                booking.DropoffLocation,
                booking.DriverId
            });
        }
    }
}
