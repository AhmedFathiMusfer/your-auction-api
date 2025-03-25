

using your_auction_api.Utility;

namespace your_auction_api.Models.Dto
{
    public class AuctionDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public AuctionState state { get; set; }

        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
    }
}