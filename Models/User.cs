using System.ComponentModel.DataAnnotations;

namespace MyTaxiService.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string FullName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password should be at least 6 characters.")]
        public required string Password { get; set; }

        [Required]
        [StringLength(50)]
        public required string Username { get; set; }
    }
}
