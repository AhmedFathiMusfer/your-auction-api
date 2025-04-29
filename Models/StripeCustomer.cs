

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class StripeCustomer
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripePaymentMethodId { get; set; }

        public ApplicationUser User { get; set; }


    }
}