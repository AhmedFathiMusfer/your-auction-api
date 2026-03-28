

using System.Linq.Expressions;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task UpdateAsync(Product obj);
        Task<PorductResponceDto> GetWithDetailsAsync(int productId);
        Task<PaginatedResult<PorductResponceDto>> GetWithDetailsAsync(ProductSpecification spec);
    }
}