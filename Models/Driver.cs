using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class Driver
    {

        [DriverId]
     public int DriverId { get; set; }

        [Name]
        public required string Name { get; set; }

        [PhoneNumber]

        public required string PhoneNumber { get; set; }
      
        public required string VehicleType { get; set; }
        public required string LicenseNumber { get; set; }
       
        public bool IsAvailable { get; set; }
        public string? CurrentLocation { get; set; }
        public double Rating { get; set; } = 5.0;


    }

    internal class PhoneNumberAttribute : Attribute
    {
    }

    internal class NameAttribute : Attribute
    {
    }

    internal class DriverIdAttribute : Attribute
    {
    }
}
