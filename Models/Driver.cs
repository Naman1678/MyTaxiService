using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public required string VehicleType { get; set; }

        [Required]
        public required string LicenseNumber { get; set; }

        public bool IsAvailable { get; set; } = true;
        public string? CurrentLocation { get; set; }
        public double Rating { get; set; } = 5.0;

        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
