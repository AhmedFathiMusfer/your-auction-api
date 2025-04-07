using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using your_auction_api.Models.Dto;

namespace your_auction_api.Validation
{
    public class RegisterationRequestDTOValidator : AbstractValidator<RegisterationRequestDTO>
    {
        public RegisterationRequestDTOValidator()
        {
            RuleFor(r => r.email).EmailAddress().NotEmpty().NotNull();
            RuleFor(r => r.Password).NotEmpty().NotNull();
        }

    }
}