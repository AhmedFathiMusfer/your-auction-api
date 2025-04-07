

using System.Linq.Expressions;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product obj);
        Task<List<PoroductResponceDto>> GetWithDetailsAsync(Expression<Func<Product, bool>>? filter = null, int pageSize = 0, int pageNumber = 1);
    }
}