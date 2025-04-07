

using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task UpdateAsync(ProductImage obj);
    }
}