

using ErrorOr;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Services.IServices
{
    public interface IAuctionService
    {
        Task<ErrorOr<PaginatedResult<Auction>>> GetAuctions(AuctionSpecification spec);
        Task<ErrorOr<Auction>> getAuctionById(int auctionId);
        Task<ErrorOr<Success>> AddAuction(AuctionDto auctionDto);
        Task<ErrorOr<Success>> AddAuctionUser(int auctionId, decimal AuctionValue);
        Task<ErrorOr<Success>> UpdateAuction(int auctionId, AuctionDto auctionDto);
        Task<ErrorOr<Success>> CanceledAuction(int auctionId);
        Task<ErrorOr<Success>> CompletedAuction(int auctionId);

        // Task<ErrorOr<String>> AddImageToProduct(IFormFile image);
        Task<ErrorOr<AuctionDetailsDto>> getAuctionWithDetails(int auctionId);
        Task<ErrorOr<PaginatedResult<AuctionDetailsDto>>> getAllAuctionsWithDetails(AuctionSpecification spec);
        Task<ErrorOr<List<AuctionUserDto>>> getAuctionUsers(int auctionId);
        Task<ErrorOr<Deleted>> DeleteAuction(int auctionId);
        Task<ErrorOr<int>> GetCountAuctions();
    }
}