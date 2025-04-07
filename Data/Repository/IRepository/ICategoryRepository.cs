
using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task UpdateAsync(Category obj);
    }
}