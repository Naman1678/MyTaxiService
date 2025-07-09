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

        /*
          POST: api/authorization/login
          Logs in as a user as Driver/Client, and validates their credentials, and returns a JWT token.
         */
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (string.IsNullOrEmpty(login.Role))
                return BadRequest("Role is required");

            int? driverId = null;

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

                driver.IsAvailable = true; 
                driverId = driver.DriverId;

                _context.SaveChanges(); 
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
                role = login.Role,
                driverId
            });
        }


        /*
          POST: api/authorization/register-client
          Registers a new client user if username doesn't already exist in the database.
         */
        [HttpPost("register-client")]
        public IActionResult RegisterClient([FromBody] User newUser)
        {
            if (_context.Users.Any(u => u.Username == newUser.Username))
                return BadRequest("Username already exists.");

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok("Client registered successfully.");
        }

        /*
          POST: api/authorization/register-driver
          Registers a new driver if username doesn't already exist.
         */
        [HttpPost("register-driver")]
        public IActionResult RegisterDriver([FromBody] Driver newDriver)
        {
            if (_context.Drivers.Any(d => d.Username == newDriver.Username))
                return BadRequest("Username already exists.");

            newDriver.IsAvailable = true;
            newDriver.CurrentLocation = "Unknown";

            _context.Drivers.Add(newDriver);
            _context.SaveChanges();

            return Ok("Driver registered successfully.");
        }

        /*
          GET: api/authorization/driver-dashboard
          Secured endpoint accessible only by Driver role to maintain authorization.
         */
        [Authorize(Roles = "Driver")]
        [HttpGet("driver-dashboard")]
        public IActionResult GetDriverDashboard()
        {
            return Ok("Welcome, driver!");
        }

        /*
          GET: api/authorization/client-dashboard
          Secured endpoint accessible only by Client role.
         */
        [Authorize(Roles = "Client")]
        [HttpGet("client-dashboard")]
        public IActionResult GetClientDashboard()
        {
            return Ok("Welcome, client!");
        }
    }
}
