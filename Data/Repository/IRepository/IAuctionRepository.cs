

using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IAuctionRepository : IRepository<Auction>
    {
        Task UpdateAsync(Auction obj);

        Task<AuctionDetailsDto> DetailsAsync(int AuctionId);

    }
}