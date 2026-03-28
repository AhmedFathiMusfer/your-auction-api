

using System.Linq.Expressions;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IAuctionRepository : IRepository<Auction>
    {
        Task UpdateAsync(Auction obj);

        Task<PaginatedResult<AuctionDetailsDto>> GetWithDetailsAsync(AuctionSpecification spec);
         Task<AuctionDetailsDto> GetWithDetailsAsync(int auctionId);
        Task<PaginatedResult<Auction>> GetAll(AuctionSpecification spec);
    }
}