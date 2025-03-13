

namespace your_auction_api.Models
{
    public class ReportArchive
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CommissionId { get; set; }
        public int PaymentId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}