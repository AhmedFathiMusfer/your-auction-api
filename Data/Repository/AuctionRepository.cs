

using System.IO.Compression;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
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



        public async Task<List<AuctionDetailsDto>> GetWithDetailsAsync(Expression<Func<Auction, bool>>? filter = null)
        {
            IQueryable<Auction> Auctions = _db.auctions;
            if (filter != null)
            {
                Auctions.Where(filter);
            }
            var detialsAuctions = Auctions.Select(a => new AuctionDetailsDto
            {
                Id = a.Id,
                ProductName = a.Product.Name,
                SalleName = a.Product.User.Name,
                StartDate = a.Start_date,
                price = a.Product.Price,
                quantity = a.Product.Quantity,
                ImagesUrl = a.Product.ProductImages.Select(i => i.ImageUrl).ToList(),
                EndDate = a.End_date,
                State = a.state.ToString(),
                NumberOfBidders = a.Users.Count()




            });

            return await detialsAuctions.ToListAsync();



        }

        public async Task UpdateAsync(Auction obj)
        {
            _db.auctions.Update(obj);
            await SaveAsync();

        }



    }
}