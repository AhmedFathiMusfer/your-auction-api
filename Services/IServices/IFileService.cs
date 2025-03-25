
using ErrorOr;
using your_auction_api.Models;

namespace your_auction_api.Services.IServices
{
    public interface IFileService
    {
        Task<ErrorOr<String>> UploadImageToTemp(IFormFile file);
        Task<ErrorOr<Success>> MoveImgeFromTempToProduct(Product product);

        Task<ErrorOr<Deleted>> DeleteImage(string ImageUrl);
    }
}