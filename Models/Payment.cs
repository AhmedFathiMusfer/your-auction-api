

namespace your_auction_api.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int AuctionId { get; set; }
        public DateTime CreationDate { get; set; }
        public Auction Auction { get; set; }
    }
}