
namespace your_auction_api.Models.Dto
{
    public class AuctionUserDto
    {
        public int Id { get; set; }
        public string userName { get; set; }

        public int aucthionId { get; set; }

        public decimal auctionValue { get; set; }
    }
}