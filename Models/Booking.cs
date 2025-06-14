﻿using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.ComponentModel.DataAnnotations;


namespace MyTaxiService.Models
{
    public class Booking
    {
        internal object? Driver;

        [BookingId]
        public int BookingId { get; set; }
        [UserId]

        public int UserId { get; set; }

        [PickupLocation]

        public required string PickupLocation { get; set; }

        [DropoffLocation]

        public required string DropoffLocation { get; set; }

        [RequestedTime]

        public DateTime RequestedTime { get; set; }

        public string? CarType { get; set; }
        public string? Status { get; set; }
        public int? DriverId { get; set; }
         
    }

    internal class RequestedTimeAttribute : Attribute
    {
    }

    internal class DropoffLocationAttribute : Attribute
    {
    }

    internal class PickupLocationAttribute : Attribute
    {
    }

    internal class UserIdAttribute : Attribute
    {
    }

    internal class BookingIdAttribute : Attribute
    {
    }
}
