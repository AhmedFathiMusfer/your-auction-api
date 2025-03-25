

using System.IO.Compression;
using Microsoft.EntityFrameworkCore.Storage;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Data.Repository
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        private readonly ApplicationDbContext _db;
        public AuctionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public async Task<AuctionDetailsDto> DetailsAsync(int AuctionId)
        {

            var datialsAuction = _db.auctions.Where(a => a.Id == AuctionId).Select(a => new AuctionDetailsDto
            {
                SalleName = a.Product.User.Name,
                StartDate = a.Start_date,
                EndDate = a.End_date,
                State = a.state.ToString(),



            }).FirstOrDefault();
            if (datialsAuction != null)
            {
                datialsAuction.NumberOfBidders = _db.auctionUsers.Where(a => a.AuctionId == AuctionId).Count();
            }
            return datialsAuction;



        }

        public async Task UpdateAsync(Auction obj)
        {
            _db.auctions.Update(obj);
            await SaveAsync();

        }



    }
}