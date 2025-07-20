using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(50, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Username { get; set; } = string.Empty;
        public ICollection<Booking>? Bookings { get; set; }
    }
}