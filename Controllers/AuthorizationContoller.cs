using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyTaxiService.Data;
using MyTaxiService.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyTaxiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (string.IsNullOrEmpty(login.Role))
                return BadRequest("Role is required");

            if (login.Role == "Client")
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == login.Username && u.Password == login.Password);
                if (user == null)
                    return Unauthorized("Invalid Client credentials");
            }
            else if (login.Role == "Driver")
            {
                var driver = _context.Drivers.FirstOrDefault(d => d.Username == login.Username && d.Password == login.Password);
                if (driver == null)
                    return Unauthorized("Invalid Driver credentials");
            }
            else
            {
                return BadRequest("Invalid role");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345_superSecureKey!789"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.Role, login.Role)
            };

            var token = new JwtSecurityToken(
                issuer: "http://localhost:5199",
                audience: "http://127.0.0.1:5500",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = login.Role
            });
        }


        [HttpPost("register-client")]
        public IActionResult RegisterClient([FromBody] User newUser)
        {
            if (_context.Users.Any(u => u.Username == newUser.Username))
            {
                return BadRequest("Username already exists.");
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("Client registered successfully.");
        }


        [HttpPost("register-driver")]
        public IActionResult RegisterDriver([FromBody] Driver newDriver)
        {
            if (_context.Drivers.Any(d => d.Username == newDriver.Username))
            {
                return BadRequest("Username already exists.");
            }

            newDriver.IsAvailable = true;
            newDriver.CurrentLocation = "Unknown";

            _context.Drivers.Add(newDriver);
            _context.SaveChanges();

            return Ok("Driver registered successfully.");
        }


        [Authorize(Roles = "Driver")]
        [HttpGet("driver-dashboard")]
        public IActionResult GetDriverDashboard()
        {
            return Ok();
        }

        [Authorize(Roles = "Client")]
        [HttpGet("client-dashboard")]
        public IActionResult GetClientDashboard()
        {
            return Ok();
        }
    }
}
