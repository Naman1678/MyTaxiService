using Microsoft.EntityFrameworkCore;
using MyTaxiService.Data;
using MyTaxiService.Models;
using MyTaxiService.Repository.Interfaces;

namespace MyTaxiService.Repository
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
