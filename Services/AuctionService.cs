
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Net.Security;
using System.Net.Http.Headers;
using System;
using System.Security.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using ErrorOr;
using FluentValidation;
using FluentValidation.TestHelper;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;
using your_auction_api.Utility;
using Microsoft.Identity.Client;

namespace your_auction_api.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAuctionUserRepository _auctionUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IValidator<AuctionDto> _auctionValidator;

        public AuctionService(IAuctionRepository auctionRepository, IAuctionUserRepository auctionUserRepository, IHttpContextAccessor httpContextAccessor, IValidator<AuctionDto> auctionValidator)
        {
            _auctionRepository = auctionRepository;
            _auctionUserRepository = auctionUserRepository;
            _httpContextAccessor = httpContextAccessor;
            _auctionValidator = auctionValidator;
        }
        public async Task<ErrorOr<Success>> AddAuction(AuctionDto auctionDto)
        {
            var product = await _auctionRepository.GetAsync(a => a.ProductId == auctionDto.ProductId, Tracked: false);
            if (product is not null)
            {
                return Error.Validation(code: "ProductId", description: "this product is already in auction");
            }
            var resultValidator = _auctionValidator.Validate(auctionDto);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }

            var auction = new Auction
            {
                ProductId = auctionDto.ProductId,
                Start_date = DateTime.UtcNow,
                state = AuctionState.Active,
                End_date = auctionDto.EndDate.ToUniversalTime(),
            };

            await _auctionRepository.CreateAsync(auction);
            return Result.Success;


        }

        public async Task<ErrorOr<Success>> AddAuctionUser(int auctionId, decimal AuctionValue)
        {

            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false, includeProperties: "Product");
            if (auction is null)
            {
                return Error.NotFound("this aucthion is not found");
            }
            if (auction.state != AuctionState.Active)
            {
                return Error.Validation(code: "state", description: $"this aucthion state is {auction.state}");
            }
            var UserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (UserId is null)
            {
                return Error.Unauthorized(description: "an Unauthorized");
            }
            if (AuctionValue < auction.Product.Price)
            {
                return Error.Validation(code: "AuctionValue", description: $"the price is unvaild becuse shold be greater than Or equal To {auction.Product.Price}");
            }
            var auctionUser = new AuctionUser
            {
                AuctionId = auctionId,
                AuctionValue = AuctionValue,
                UserId = UserId,
                startDate = DateTime.Now
            };

            await _auctionUserRepository.CreateAsync(auctionUser);
            return Result.Success;
        }



        public async Task<ErrorOr<Success>> CanceledAuction(int auctionId)
        {
            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false);
            if (auction is null)
            {
                return Error.NotFound("this aucthion is not found");
            }
            var UserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (UserId is null)
            {
                return Error.Unauthorized(description: "an Unauthorized");
            }
            if (auction.state != AuctionState.Active)
            {
                return Error.Validation(code: "state", description: $"this aucthion state is {auction.state}");
            }
            auction.state = AuctionState.Canceled;
            await _auctionRepository.UpdateAsync(auction);
            return Result.Success;
        }



        public async Task<ErrorOr<Deleted>> DeleteAuction(int auctionId)
        {
            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false);
            if (auction is null)
            {
                return Error.NotFound("this auction not found");
            }

            await _auctionRepository.RemoveAsync(auction);
            return Result.Deleted;
        }

        public async Task<ErrorOr<AuctionDetailsDto>> getAuctionWithDetails(int auctionId)
        {
            var auctionDetails = (await _auctionRepository.GetWithDetailsAsync(a => a.Id == auctionId)).FirstOrDefault();
            if (auctionDetails is null)
            {
                return Error.NotFound("this auction not found");
            }
            return auctionDetails;
        }

        public async Task<ErrorOr<Auction>> getAuctionById(int auctionId)
        {
            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false);
            if (auction is null)
            {
                return Error.NotFound("this auction not found");
            }
            return auction;
        }

        public async Task<ErrorOr<List<Auction>>> GetAuctions()
        {
            // changeStateAuctionWhenEndDate();
            var auctions = await _auctionRepository.GetAllAsync();
            return auctions;
        }


        public async Task<ErrorOr<List<AuctionUserDto>>> getAuctionUsers(int AuctionId)
        {
            var auctionUsers = await _auctionUserRepository.GetByAuctionId(AuctionId);

            return auctionUsers;
        }

        public async Task<ErrorOr<Success>> UpdateAuction(int auctionId, AuctionDto auctionDto)
        {
            var oldAuction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false);
            if (oldAuction is null || auctionId != auctionDto.Id)
            {
                return Error.NotFound("this auction not found");
            }
            var resultValidator = _auctionValidator.Validate(auctionDto);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }
            var auction = new Auction
            {
                Id = auctionDto.Id,
                ProductId = auctionDto.ProductId,
                End_date = auctionDto.EndDate
            };

            await _auctionRepository.UpdateAsync(auction);
            return Result.Success;
        }
        private void changeStateAuctionWhenEndDate()
        {
            var auctions = _auctionRepository.GetAllAsync(a => a.End_date < DateTime.Now && a.state == AuctionState.Active).Result;
            foreach (var auction in auctions)
            {
                auction.state = AuctionState.Completed;

                _auctionRepository.UpdateAsync(auction).Wait();



            }


        }

        public async Task<ErrorOr<List<AuctionDetailsDto>>> getAllAuctionsWithDetails()
        {
            var aucthionsWithDetails = await _auctionRepository.GetWithDetailsAsync();
            return aucthionsWithDetails;
        }

        public async Task<ErrorOr<Success>> CompletedAuction(int auctionId)
        {
            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionId, Tracked: false);
            if (auction is null)
            {
                return Error.NotFound(description: "theis auction not found");
            }
            if (!(auction.state == AuctionState.Active && auction.End_date <= DateTime.Now))
            {
                return Error.Validation("error in date");
            }
            auction.state = AuctionState.Completed;
            await _auctionRepository.UpdateAsync(auction);
            return Result.Success;
        }
    }
}