


namespace your_auction_api.Models.Dto
{
    public class AuctionDetailsDto
    {
        public string ProductName { get; set; }
        public string SalleName { get; set; }
        public int NumberOfBidders { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        public string State { get; set; }
    }
}