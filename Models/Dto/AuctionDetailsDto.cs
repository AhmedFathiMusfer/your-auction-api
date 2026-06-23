


namespace your_auction_api.Models.Dto
{
    public class AuctionDetailsDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string SalleName { get; set; }
        public string Description { get; set; }
        public int NumberOfBidders { get; set; }
        public DateTime StartDate { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public DateTime EndDate { get; set; }
        public string State { get; set; }
        public List<string> ImagesUrl { get; set; }

    }
}