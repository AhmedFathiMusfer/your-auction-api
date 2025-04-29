
using FluentValidation;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Validation
{
    public class AuctionValidation : AbstractValidator<AuctionDto>
    {
        public AuctionValidation()
        {
            RuleFor(a => a.EndDate.ToUniversalTime()).NotEmpty().GreaterThan(DateTime.UtcNow);
        }
    }
}