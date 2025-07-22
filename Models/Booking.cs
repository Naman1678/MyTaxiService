using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace MyTaxiService.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public string PickupLocation { get; set; } = string.Empty;

        [Required]
        public string DropoffLocation { get; set; } = string.Empty;

        public DateTime RequestedTime { get; set; }
        public string? CarType { get; set; }
        public string? Status { get; set; }

        public int? DriverId { get; set; }

        [ForeignKey("DriverId")]
        public Driver? Driver { get; set; }
    }
}
