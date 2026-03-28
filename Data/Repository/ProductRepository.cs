

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PaginatedResult<PorductResponceDto>> GetWithDetailsAsync(ProductSpecification spec)
        {
            var products = await ApplySpecification(spec);
            var porductsDetails = await products.Select(p => new PorductResponceDto
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
            var result = new PaginatedResult<PorductResponceDto>(
                  porductsDetails,
                  spec.TotalCount,
                  spec.PageNumber,
                  spec.PageSize
              );

            return result;
        }

        public async Task<PorductResponceDto> GetWithDetailsAsync(int productId)
        {
            var product = await _db.products.Where(p => p.Id == productId).Select(p => new PorductResponceDto
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
            }).FirstOrDefaultAsync();
            return product;
        }

        public async Task UpdateAsync(Product obj)
        {
            _db.products.Update(obj);
            await SaveAsync();

        }
        private async Task<IQueryable<Product>> ApplySpecification(ProductSpecification spec)
        {
            IQueryable<Product> products = _db.products;
            if (!string.IsNullOrEmpty(spec.Search))
            {
                products = products.Where(a => a.Name.ToLower().Contains(spec.Search.ToLower()));
            }
            if (spec.PageNumber > 0 && spec.PageSize > 0)
            {
                spec.TotalCount = await products.CountAsync();
                if (spec.PageSize > 100)
                {
                    spec.PageSize = 100;
                }
            }
            else
            {
                spec.PageNumber = 1;
                spec.PageSize = 10;

            }

            if (string.IsNullOrEmpty(spec.OrderBy))
            {
                products = products.OrderByDescending(a => a.Id);
            }
            else
            {
                if (spec.OrderDirection == "desc")
                {
                    products = products.OrderByDescending(a => EF.Property<object>(a, spec.OrderBy));
                }
                else
                {
                    products = products.OrderBy(a => EF.Property<object>(a, spec.OrderBy));
                }
            }

            products = products.Skip(spec.PageSize * (spec.PageNumber - 1)).Take(spec.PageSize);
            return products;
        }



    }
}