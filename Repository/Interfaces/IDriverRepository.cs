using MyTaxiService.Models;

namespace MyTaxiService.Repository.Interfaces
{
    public interface IDriverRepository
    {
        void AddDriver(Driver driver);
        Task<Driver?> GetDriverByIdAsync(int id);
        void Save();
    }
}
