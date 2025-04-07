using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using your_auction_api.Models.Dto;

namespace your_auction_api.Validation
{
    public class LoginRequestDTOValidation : AbstractValidator<LoginRequestDTO>
    {
        public LoginRequestDTOValidation()
        {
            RuleFor(l => l.email).EmailAddress().NotEmpty().NotNull();
            RuleFor(l => l.Password).NotEmpty().NotNull();
        }

    }
}