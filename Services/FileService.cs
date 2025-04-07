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

        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductRepository _productRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private string basePath;
        public FileService(IWebHostEnvironment webHostEnvironment,
        IProductImageRepository productImageRepository,
         IHttpContextAccessor httpContextAccessor,
         IProductRepository productRepository)
        {
            _webHostEnvironment = webHostEnvironment;
            _productImageRepository = productImageRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;

            basePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
        }

        public async Task<ErrorOr<Deleted>> DeleteImageFromProduct(int ProductId, string ImageUrl)
        {
            var productImage = await _productImageRepository.GetAsync(pi => pi.ProductId == ProductId && pi.ImageUrl == ImageUrl);
            if (productImage is null)
            {
                return Error.Validation(code: "images", description: "fial when delete the image");
            }
            var ImgeBath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "products", ProductId.ToString(), Path.GetFileName(ImageUrl));
            if (File.Exists(ImgeBath))
            {
                File.Delete(ImgeBath);
            }
            await _productImageRepository.RemoveAsync(productImage);
            return Result.Deleted;
        }

        public async Task<ErrorOr<Deleted>> DeleteImageFromTemp(string ImageUrl)
        {
            var tempFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "temp", Path.GetFileName(ImageUrl));
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
            return Result.Deleted; ;
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
                    await _productImageRepository.CreateAsync(new ProductImage
                    {
                        ImageUrl = $"{basePath}/Images/products/{product.Id}/{Path.GetFileName(image)}",
                        ProductId = product.Id

                    });

                }


            }


            return Result.Success;
        }

        public async Task<ErrorOr<string>> UploadImageToProduct(IFormFile file, int ProductId)
        {
            if (file is null)
            {
                return Error.Validation(code: "image", description: "fill in uploade image");
            }
            var product = await _productRepository.GetAsync(p => p.Id == ProductId);
            if (product is null)
            {
                return Error.Validation(code: "image", description: "the product not found");
            }
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "products", ProductId.ToString());
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var FileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(folderPath, FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            await _productImageRepository.CreateAsync(new ProductImage
            {
                ImageUrl = $"{basePath}/Images/products/{ProductId}/{FileName}",
                ProductId = ProductId

            });

            return $"{basePath}/Images/products/{ProductId}/{file.FileName}";
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
            var FileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

            var filePath = Path.Combine(folderPath, FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);


            }

            return $"{basePath}/Images/temp/{FileName}";




        }
    }
}