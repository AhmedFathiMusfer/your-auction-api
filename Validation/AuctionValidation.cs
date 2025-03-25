
using FluentValidation;
using your_auction_api.Models;
using your_auction_api.Models.Dto;

namespace your_auction_api.Validation
{
    public class AuctionValidation : AbstractValidator<AuctionDto>
    {
        public AuctionValidation()
        {


            RuleFor(a => a.Start_date).NotEmpty().GreaterThanOrEqualTo(DateTime.Now);
            RuleFor(a => a.End_date).NotEmpty().GreaterThan(a => a.Start_date);
            RuleFor(a => a.state).IsInEnum();


        }
    }
}