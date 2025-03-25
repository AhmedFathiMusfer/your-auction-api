

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class Payment
    {
        public int Id { get; set; }
        [ForeignKey("AuctionUser")]
        public int AuctionUserId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime CreationDate { get; set; }
        public AuctionUser AuctionUser { get; set; }
    }
}