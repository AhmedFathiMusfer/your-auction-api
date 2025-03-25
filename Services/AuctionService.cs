
using System.IdentityModel.Tokens.Jwt;
using ErrorOr;
using FluentValidation;
using FluentValidation.TestHelper;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;
using your_auction_api.Utility;

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
            var resultValidator = _auctionValidator.Validate(auctionDto);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll(error => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();
                return error;
            }
            var auction = new Auction
            {
                ProductId = auctionDto.ProductId,
                state = auctionDto.state,
                Start_date = auctionDto.Start_date,
                End_date = auctionDto.End_date
            };
            await _auctionRepository.CreateAsync(auction);
            return Result.Success;


        }

        public async Task<ErrorOr<Success>> AddAuctionUser(AuctionUser auctionUser)
        {
            var auction = await _auctionRepository.GetAsync(a => a.Id == auctionUser.AuctionId, Tracked: false, includeProperties: "Product");
            if (auction is null)
            {
                return Error.NotFound("this aucthion is not found");
            }
            if (auction.state != AuctionState.Active)
            {
                return Error.Validation(code: "state", description: $"this aucthion state is {auction.state}");
            }
            var UserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            if (UserId is null)
            {
                return Error.Unauthorized(description: "an Unauthorized");
            }
            if (auctionUser.AuctionValue < auction.Product.Price)
            {
                return Error.Validation(code: "AuctionValue", description: $"the price is unvaild becuse shold be greater than Or equal To {auction.Product.Price}");
            }
            auctionUser.UserId = UserId;
            auctionUser.startDate = DateTime.Now;
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
            var UserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
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

        public async Task<ErrorOr<AuctionDetailsDto>> detailsAuction(int auctionId)
        {
            var auctionDetails = await _auctionRepository.DetailsAsync(auctionId);
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
                state = auctionDto.state,
                Start_date = auctionDto.Start_date,
                End_date = auctionDto.End_date
            };

            await _auctionRepository.UpdateAsync(auction);
            return Result.Success;
        }
    }
}