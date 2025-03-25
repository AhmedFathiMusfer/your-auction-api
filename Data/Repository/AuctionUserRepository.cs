
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository
{

    public class AuctionUserRepository : Repository<AuctionUser>, IAuctionUserRepository
    {
        private readonly ApplicationDbContext _db;
        public AuctionUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public async Task UpdateAsync(AuctionUser obj)
        {
            _db.auctionUsers.Update(obj);
            await SaveAsync();

        }

        public async Task<List<AuctionUserDto>> GetByAuctionId(int AuctionId)
        {
            var auctionUsers = _db.auctionUsers.Where(au => au.AuctionId == AuctionId).Select(au => new AuctionUserDto
            {
                Id = au.Id,
                userName = au.User.Name,
                auctionValue = au.AuctionValue
            }).ToList();
            return auctionUsers;
        }
    }
}