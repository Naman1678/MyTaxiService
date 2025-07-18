using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.ComponentModel.DataAnnotations;


namespace MyTaxiService.Models
{
    public class Booking
    {
        internal object? Driver;

        public int BookingId { get; set; }
        public int UserId { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }
        public DateTime RequestedTime { get; set; }
        public string? CarType { get; set; }
        public string? Status { get; set; }
        public int? DriverId { get; set; }

    }

}