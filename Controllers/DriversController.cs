using Microsoft.AspNetCore.Mvc;
using MyTaxiService.Data;
using MyTaxiService.Models;

namespace MyTaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DriversController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDriver([FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            driver.IsAvailable = true;
            driver.CurrentLocation = "Unknown";

            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDriver), new { id = driver.DriverId }, driver);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
                return NotFound();

            return Ok(driver);
        }
    }
}
