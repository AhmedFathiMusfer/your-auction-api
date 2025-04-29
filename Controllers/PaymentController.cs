using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Controllers
{

    [Route("api/[controller]")]
    [Authorize]
    public class PaymentController : ApiController
    {
        private readonly IStripeCustomerService _stripeCustomerService;
        public PaymentController(IStripeCustomerService stripeCustomerService)
        {
            _stripeCustomerService = stripeCustomerService;
        }


        [HttpGet("GetPaymentStatus")]
        public async Task<IActionResult> GetPaymentStatus()
        {
            var result = await _stripeCustomerService.GetPaymentStatus();

            return result.Match(
               status => Ok(new { needCard = status }),
                  Problem
              );

        }
        [HttpPost("CreateSetupIntent")]
        public async Task<IActionResult> CreateSetupIntent()
        {
            var result = await _stripeCustomerService.CreateSetupIntent();

            return result.Match(
               setupIntent => Ok(setupIntent),
                  Problem
              );
        }
        [HttpPost("SavePaymentMethod")]
        public async Task<IActionResult> SavePaymentMethod([FromBody] SavePaymentDto savePaymentDto)
        {
            var result = await _stripeCustomerService.SavePaymentMethod(savePaymentDto);

            return result.Match(
               status => Created(),
                  Problem
              );
        }
    }
}