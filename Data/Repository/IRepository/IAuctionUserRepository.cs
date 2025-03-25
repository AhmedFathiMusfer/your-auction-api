

using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IAuctionUserRepository : IRepository<AuctionUser>
    {
        Task UpdateAsync(AuctionUser obj);

        Task<List<AuctionUserDto>> GetByAuctionId(int AuctionId);



    }
}