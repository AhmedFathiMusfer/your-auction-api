

using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using ErrorOr;
using Stripe;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models.Dto;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class StripeCustomerService : IStripeCustomerService
    {

        private readonly IStripeCustomerRepository _stripeCustomerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StripeCustomerService(IStripeCustomerRepository stripeCustomerRepository, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _stripeCustomerRepository = stripeCustomerRepository;

        }


        public async Task<ErrorOr<CreateSetupIntentDto>> CreateSetupIntent()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(ct => ct.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Error.NotFound("user not found");
            }
            var stripeCustomer = _stripeCustomerRepository.GetAsync(x => x.UserId == userId).Result;
            var customerService = new CustomerService();
            string customerId;
            if (stripeCustomer != null)
            {
                customerId = stripeCustomer.StripeCustomerId;
                return new CreateSetupIntentDto
                {

                    CustomerId = customerId
                };
            }
            else
            {
                var customer = await customerService.CreateAsync(new CustomerCreateOptions
                {
                    Description = $"User {userId}"
                });

                customerId = customer.Id;


            }
            var setupIntentService = new SetupIntentService();
            var setupIntent = await setupIntentService.CreateAsync(new SetupIntentCreateOptions
            {
                Customer = customerId,
                PaymentMethodTypes = new List<string> { "card" },
            });
            return new CreateSetupIntentDto
            {
                ClientSecret = setupIntent.ClientSecret,
                CustomerId = customerId
            };



        }



        public async Task<ErrorOr<bool>> GetPaymentStatus()
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(ct => ct.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Error.NotFound("user not found");
            }
            var stripeCustomer = await _stripeCustomerRepository.GetAsync(x => x.UserId == userId);
            if (stripeCustomer == null)
            {
                return false;
            }
            return true;
        }

        public async Task<ErrorOr<Success>> SavePaymentMethod(SavePaymentDto savePaymentDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(ct => ct.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Error.NotFound("user not found");
            }
            var stripeCustomer = await _stripeCustomerRepository.GetAsync(x => x.UserId == userId);
            if (stripeCustomer != null)
            {
                stripeCustomer.StripePaymentMethodId = savePaymentDto.paymentMethodId;
                stripeCustomer.StripeCustomerId = savePaymentDto.CustomerId;
            }
            else
            {
                stripeCustomer = new Models.StripeCustomer
                {
                    StripePaymentMethodId = savePaymentDto.paymentMethodId,
                    StripeCustomerId = savePaymentDto.CustomerId,
                    UserId = userId
                };
                await _stripeCustomerRepository.CreateAsync(stripeCustomer);
            }

            return Result.Success;
        }


    }
}