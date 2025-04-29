

namespace your_auction_api.Models.Dto
{
    public class SavePaymentDto
    {
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string paymentMethodId{ get; set; }
    }
}