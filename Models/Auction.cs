

namespace your_auction_api.Models
{
    public class Auction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public decimal AuctionValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
    }
}