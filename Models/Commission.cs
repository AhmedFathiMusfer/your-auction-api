

namespace your_auction_api.Models
{
    public class Commission
    {
        public int Id { get; set; }
        public decimal CommissionValue { get; set; }
        public int PaymentId { get; set; }
        public DateTime CreationDate { get; set; }
    }
}