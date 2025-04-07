
using ErrorOr;
using your_auction_api.Models;

namespace your_auction_api.Services.IServices
{
    public interface IFileService
    {
        Task<ErrorOr<String>> UploadImageToTemp(IFormFile file);
        Task<ErrorOr<Success>> MoveImgeFromTempToProduct(Product product);

        Task<ErrorOr<Deleted>> DeleteImageFromProduct(int ProductId, string ImageUrl);
        Task<ErrorOr<Deleted>> DeleteImageFromTemp(string ImageUrl);
        Task<ErrorOr<String>> UploadImageToProduct(IFormFile file, int ProductId);

    }
}