

using System.IdentityModel.Tokens.Jwt;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        private readonly IProductImageRepository _productImageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileService _fileService;
        private IValidator<ProductDto> _productValidator;

        public ProductService(IValidator<ProductDto> productValidator, IProductRepository productRepository,
         IProductImageRepository productImageRepository, IFileService fileService,
         IHttpContextAccessor httpContextAccessor)
        {
            _productValidator = productValidator;
            _productRepository = productRepository;
            _productImageRepository = productImageRepository;
            _fileService = fileService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ErrorOr<String>> AddImageToProduct(IFormFile image)
        {
            var result = await _fileService.UploadImageToTemp(image);
            if (result.IsError)
            {
                return result.Errors;
            }
            return result;
        }

        public async Task<ErrorOr<Success>> AddProduct(ProductDto productDto)
        {
            var resultValidator = _productValidator.Validate(productDto);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }
            var product = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price,
                Quantity = productDto.Quantity,

                IsChecked = productDto.IsChecked,
                images = productDto.images
            };
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            product.UserId = userId;

            await _productRepository.CreateAsync(product);
            var result = await _fileService.MoveImgeFromTempToProduct(product);
            if (result.IsError)
            {
                return result.Errors;
            }

            return Result.Success;
        }

        public async Task<ErrorOr<Deleted>> DeleteProduct(int productId)
        {

            var product = await _productRepository.GetAsync(p => p.Id == productId);
            if (product == null)
            {
                return Error.NotFound(description: "the product  not found");
            }
            await _productRepository.RemoveAsync(product);
            return Result.Deleted;
        }

        public async Task<ErrorOr<Product>> getProductById(int productId)
        {
            var product = await _productRepository.GetAsync(p => p.Id == productId);
            if (product == null)
            {
                return Error.NotFound(description: "the product  not found");
            }

            return product;
        }

        public async Task<ErrorOr<List<Product>>> GetProducts()
        {
            var products = await _productRepository.GetAllAsync();

            return products;
        }

        public async Task<ErrorOr<Success>> UpdateProduct(int productId, ProductDto productDto)
        {
            var Oldproduct = await _productRepository.GetAsync(p => p.Id == productId, Tracked: false);

            if (Oldproduct is null)
            {
                return Error.NotFound(description: "the product in not null");
            }
            var resultValidator = _productValidator.Validate(productDto);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }


            Oldproduct.Name = productDto.Name;
            Oldproduct.Description = productDto.Description;
            Oldproduct.CategoryId = productDto.CategoryId;
            Oldproduct.Price = productDto.Price;
            Oldproduct.Quantity = productDto.Quantity;

            Oldproduct.IsChecked = productDto.IsChecked;
            Oldproduct.images = productDto.images;

            await _productRepository.UpdateAsync(Oldproduct);

            return Result.Success;


        }
    }
}