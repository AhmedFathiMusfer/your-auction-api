

using ErrorOr;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Services.IServices
{
    public interface IAuctionService
    {
        Task<ErrorOr<List<Auction>>> GetAuctions();
        Task<ErrorOr<Auction>> getAuctionById(int auctionId);
        Task<ErrorOr<Success>> AddAuction(AuctionDto auctionDto);
        Task<ErrorOr<Success>> AddAuctionUser(int auctionId, decimal AuctionValue);
        Task<ErrorOr<Success>> UpdateAuction(int auctionId, AuctionDto auctionDto);
        Task<ErrorOr<Success>> CanceledAuction(int auctionId);

        // Task<ErrorOr<String>> AddImageToProduct(IFormFile image);
        Task<ErrorOr<AuctionDetailsDto>> detailsAuction(int auctionId);
        Task<ErrorOr<List<AuctionUserDto>>> getAuctionUsers(int auctionId);
        Task<ErrorOr<Deleted>> DeleteAuction(int auctionId);
    }
}