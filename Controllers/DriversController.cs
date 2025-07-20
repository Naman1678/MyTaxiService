using Microsoft.AspNetCore.Mvc;
using MyTaxiService.Models;
using MyTaxiService.Repository.Interfaces;

namespace MyTaxiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController(IDriverRepository repo) : ControllerBase
    {
        private readonly IDriverRepository _repo = repo;

        [HttpPost]
        public IActionResult RegisterDriver([FromBody] Driver driver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _repo.AddDriver(driver);
            _repo.Save();
            return Ok(driver);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDriver(int id)
        {
            var driver = await _repo.GetDriverByIdAsync(id);
            if (driver == null)
                return NotFound();

            return Ok(driver);
        }
    }
}
