﻿using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }  

        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;

        public ICollection<Booking>? Bookings { get; set; }  
    }
}
