

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class Commission
    {
        public int Id { get; set; }
        public decimal CommissionValue { get; set; }
        [ForeignKey("payment")]
        public int PaymentId { get; set; }
        public DateTime CreationDate { get; set; }

        public Payment payment { get; set; }


    }
}