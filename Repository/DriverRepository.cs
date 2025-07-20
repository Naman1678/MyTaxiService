using MyTaxiService.Data;
using MyTaxiService.Models;
using MyTaxiService.Repository.Interfaces;

namespace MyTaxiService.Repository
{
    public class DriverRepository(AppDbContext context) : IDriverRepository
    {
        private readonly AppDbContext _context = context;

        public void AddDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
        }

        public async Task<Driver?> GetDriverByIdAsync(int id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
