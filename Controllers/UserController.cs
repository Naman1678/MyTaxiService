using Microsoft.AspNetCore.Mvc;
using MyTaxiService.Models;
using MyTaxiService.Repository.Interfaces;

namespace MyTaxiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(IUserRepository repo) : ControllerBase
    {
        private readonly IUserRepository _repo = repo;

        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repo.AddUserAsync(user);
            await _repo.SaveAsync();

            return Ok("User registered successfully");
        }
    }
}
  