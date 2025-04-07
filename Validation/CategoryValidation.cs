

using FluentValidation;
using your_auction_api.Models;

namespace your_auction_api.Validation
{
    public class CategoryValidation : AbstractValidator<Category>
    {
        public CategoryValidation()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Description).NotEmpty().MinimumLength(30);

        }
    }
}