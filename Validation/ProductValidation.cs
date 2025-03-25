using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Utility;

namespace your_auction_api.Validation
{
    public class ProductValidation : AbstractValidator<ProductDto>
    {
        public ProductValidation()
        {


            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Quantity).GreaterThanOrEqualTo(1);
            RuleFor(p => p.Price).GreaterThan(1);
            RuleFor(p => p.CategoryId).GreaterThan(0);

        }
    }
}