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

            _context.Drivers.Add(driver);
            _context.SaveChanges();
            return Ok(driver);
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
