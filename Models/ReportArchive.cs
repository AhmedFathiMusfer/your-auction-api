

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class ReportArchive
    {
        public int Id { get; set; }

        [ForeignKey("auction")]
        public int AuctionId { get; set; }

        [ForeignKey("commission")]
        public int CommissionId { get; set; }
        [ForeignKey("payment")]
        public int PaymentId { get; set; }
        public DateTime CreationDate { get; set; }

        public Payment payment { get; set; }
        public Commission commission { get; set; }
        public Auction auction { get; set; }
    }
}