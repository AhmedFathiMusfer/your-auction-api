

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class AuctionUser
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        [ForeignKey("Auction")]
        public int AuctionId { get; set; }
        public decimal AuctionValue { get; set; }
        public Auction Auction { get; set; }

        public DateTime startDate{get;set;}
        public ApplicationUser User { get; set; }


    }
}