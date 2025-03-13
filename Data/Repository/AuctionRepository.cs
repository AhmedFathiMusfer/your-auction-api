

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        private readonly ApplicationDbContext _db;
        public AuctionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Auction obj)
        {
            _db.auctions.Update(obj);
            await SaveAsync();

        }


    }
}