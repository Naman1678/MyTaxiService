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
        public IActionResult RegisterDriver([FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            driver.IsAvailable = true;
            driver.CurrentLocation = "Unknown";
            _context.Drivers.Add(driver);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetDriver), new { id = driver.DriverId }, driver);
        }

        [HttpGet("{id}")]
        public IActionResult GetDriver(int id)
        {
            var driver = _context.Drivers.Find(id);
            if (driver == null)
                return NotFound();
            return Ok(driver);
        }
    }
}
