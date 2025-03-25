

using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product obj);
    }
}