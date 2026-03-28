

using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository
{
    public class AuctionRepository : Repository<Auction>, IAuctionRepository
    {
        private readonly ApplicationDbContext _db;
        public AuctionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PaginatedResult<Auction>> GetAll(AuctionSpecification spec)
        {
            {
                IQueryable<Auction> Auctions = await ApplySpecification(spec);


                var result = new PaginatedResult<Auction>(
                    await Auctions.ToListAsync(),
                    spec.TotalCount,
                    spec.PageNumber,
                    spec.PageSize
                );


                return result;
            }

        }

        public async Task<PaginatedResult<AuctionDetailsDto>> GetWithDetailsAsync(AuctionSpecification spec)
        {
            IQueryable<Auction> Auctions = await ApplySpecification(spec);
            var auctionDetails = Auctions.Select(a => new AuctionDetailsDto
            {
                Id = a.Id,
                ProductName = a.Product.Name,
                SalleName = a.Product.User.Name,
                StartDate = a.Start_date,
                price = a.Product.Price,
                quantity = a.Product.Quantity,
                ImagesUrl = a.Product.ProductImages.Select(i => i.ImageUrl).ToList(),
                EndDate = a.End_date,
                State = a.status.ToString(),
                NumberOfBidders = a.Users.Count()




            });

            var result = new PaginatedResult<AuctionDetailsDto>(
                   await auctionDetails.ToListAsync(),
                   spec.TotalCount,
                   spec.PageNumber,
                   spec.PageSize
               );


            return result;



        }
        public async Task<AuctionDetailsDto> GetWithDetailsAsync(int auctionId)
        {
            var auctionDetails = _db.auctions.Where(a => a.Id == auctionId).Select(a => new AuctionDetailsDto
            {
                Id = a.Id,
                ProductName = a.Product.Name,
                SalleName = a.Product.User.Name,
                StartDate = a.Start_date,
                price = a.Product.Price,
                quantity = a.Product.Quantity,
                ImagesUrl = a.Product.ProductImages.Select(i => i.ImageUrl).ToList(),
                EndDate = a.End_date,
                State = a.status.ToString(),
                NumberOfBidders = a.Users.Count()




            });
            return auctionDetails.FirstOrDefault();
        }

        public async Task UpdateAsync(Auction obj)
        {
            _db.auctions.Update(obj);
            await SaveAsync();

        }

        private async Task<IQueryable<Auction>> ApplySpecification(AuctionSpecification spec)
        {
            IQueryable<Auction> Auctions = _db.auctions;
            if (!string.IsNullOrEmpty(spec.Search))
            {
                Auctions = Auctions.Where(a => a.Product.Name.ToLower().Contains(spec.Search.ToLower()));
            }
            if (spec.PageNumber > 0 && spec.PageSize > 0)
            {
                spec.TotalCount = await Auctions.CountAsync();
                if (spec.PageSize > 100)
                {
                    spec.PageSize = 100;
                }
            }
            else
            {
                spec.PageNumber = 1;
                spec.PageSize = 10;

            }

            if (string.IsNullOrEmpty(spec.OrderBy))
            {
                Auctions = Auctions.OrderByDescending(a => a.Start_date);
            }
            else
            {
                if (spec.OrderDirection == "desc")
                {
                    Auctions = Auctions.OrderByDescending(a => EF.Property<object>(a.Product, spec.OrderBy));
                }
                else
                {
                    Auctions = Auctions.OrderBy(a => EF.Property<object>(a, spec.OrderBy));
                }
            }

            Auctions = Auctions.Skip(spec.PageSize * (spec.PageNumber - 1)).Take(spec.PageSize);
            return Auctions;
        }



    }
}