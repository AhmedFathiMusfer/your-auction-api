

using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IUserRepository
    {
        Task UpdateAsync(ApplicationUser obj);
    }
}