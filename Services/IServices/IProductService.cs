using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Services.IServices
{
    public interface IProductService
    {
        Task<ErrorOr<PaginatedResult<PorductResponceDto>>> GetProducts(ProductSpecification spec);
        Task<ErrorOr<PorductResponceDto>> getProductById(int productId);
        Task<ErrorOr<Success>> AddProduct(ProductDto productDto);

        Task<ErrorOr<Success>> UpdateProduct(int productId, ProductDto productDto);

        Task<ErrorOr<Deleted>> DeleteProduct(int productId);
        Task<ErrorOr<int>> GetCountProducts();
    }
}