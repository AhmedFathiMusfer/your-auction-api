using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Services.IServices
{
    public interface IProductService
    {
        Task<ErrorOr<List<Product>>> GetProducts();
        Task<ErrorOr<Product>> getProductById(int productId);
        Task<ErrorOr<Success>> AddProduct(ProductDto productDto);

        Task<ErrorOr<Success>> UpdateProduct(int productId, ProductDto productDto);
        Task<ErrorOr<String>> AddImageToProduct(IFormFile image);
        Task<ErrorOr<Deleted>> DeleteProduct(int productId);
    }
}