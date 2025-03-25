using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ErrorOr;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FileService(IWebHostEnvironment webHostEnvironment,
        IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ErrorOr<Deleted>> DeleteImage(string ImageUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<Success>> MoveImgeFromTempToProduct(Product product)
        {
            if (product is null)
            {
                return Error.Validation(code: "images", description: "fial when uplode the images");
            }
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "products", product.Id.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            foreach (var image in product.images)
            {
                var tempFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "temp", Path.GetFileName(image));
                var newFilePath = Path.Combine(folderPath, Path.GetFileName(image));
                if (File.Exists(tempFilePath))
                {
                    File.Move(tempFilePath, newFilePath);
                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = newFilePath
                    });
                }


            }
            await _productRepository.UpdateAsync(product);

            return Result.Success;
        }

        public async Task<ErrorOr<string>> UploadImageToTemp(IFormFile image)
        {
            if (image is null)
            {
                return Error.Validation(code: "image", description: "fill in uploade image");
            }
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "temp");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);


            }
            var basePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
            return $"{basePath}/Images//temp/{image.FileName}";




        }
    }
}