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

        /* Creates a new ride booking. The default status is set to "Pending".
        <param name="booking">Booking data sent from the client
        returns 201 error and created with the new booking info or 400 Bad Request on validation failure
        */
        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = _service.CreateBooking(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = created.BookingId }, created);
        }

        /* GET: api/bookings/{id}
        Fetches the specific booking by ID, including driver information
         <returns>200 OK with booking data or 404 Not Found
        */
        [HttpGet("{id}")]
        public IActionResult GetBooking(int id)
        {
            var booking = _service.GetBookingById(id);
            if (booking == null) return NotFound();
            return Ok(booking);
        }

        /*
         Returns all bookings that are still pending. Requires Driver authorization.
        */
        [HttpGet("pending")]
        [Authorize(Roles = "Driver")]
        public IActionResult GetPendingBookings()
        {
            var pending = _service.GetPendingBookings();
            return Ok(pending);
        }

        /*
         Accepts a pending booking and assigns it to the specified driver. If the driver is unavailabe it marks that driver and returns booking 
         cannot be accepted.
        */

        [HttpPut("{id}/accept")]
        [Authorize(Roles = "Driver")]
        public IActionResult AcceptBooking(int id, [FromQuery] int driverId)
        {
            var updated = _service.AcceptBooking(id, driverId);
            if (updated == null) return BadRequest("Booking cannot be accepted.");
            return Ok("Booking accepted.");
        }

        /*
        Declines a pending booking and marks it as "Cancelled". Also, returns a Bad Request if booking cannot be declined.
        */

        [HttpPut("{id}/decline")]
        [Authorize(Roles = "Driver")]
        public IActionResult DeclineBooking(int id)
        {
            var updated = _service.DeclineBooking(id);
            if (updated == null) return BadRequest("Booking cannot be declined.");
            return Ok("Booking declined.");
        }
    }
}
