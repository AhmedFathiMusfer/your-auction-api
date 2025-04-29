

using ErrorOr;
using your_auction_api.Models.Dto;

namespace your_auction_api.Services.IServices
{
    public interface IStripeCustomerService
    {
       
        Task<ErrorOr<bool>> GetPaymentStatus();
        Task<ErrorOr<CreateSetupIntentDto>> CreateSetupIntent();
        Task<ErrorOr<Success>> SavePaymentMethod(SavePaymentDto savePaymentDto);
    }

}