using MyTaxiService.Models;

namespace MyTaxiService.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task SaveAsync();
    }
}
