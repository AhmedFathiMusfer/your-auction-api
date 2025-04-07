

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<PoroductResponceDto>> GetWithDetailsAsync(Expression<Func<Product, bool>>? filter = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<Product> query = _db.products;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }
            var result = await query.Select(p => new PoroductResponceDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Price = p.Price,
                sellerName = p.User.Name,
                IsChecked = p.IsChecked,
                Emp_note = p.Emp_note,
                CategoryName = p.Category.Name,
                CategoryId = p.CategoryId,
                images = p.ProductImages.Select(pi => pi.ImageUrl).ToList()
            }).ToListAsync();
            return result;
        }

        public async Task UpdateAsync(Product obj)
        {
            _db.products.Update(obj);
            await SaveAsync();

        }


    }
}