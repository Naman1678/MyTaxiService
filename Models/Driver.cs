using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class Driver
    {
        public int DriverId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        public string CarNumber { get; set; } = string.Empty;

        [Required]
        public string LicenseNumber { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
        public string? CurrentLocation { get; set; }
        public double Rating { get; set; } = 5.0;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public ICollection<Booking>? Bookings { get; set; }
    }
}