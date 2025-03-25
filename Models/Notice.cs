

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class Notice
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public ApplicationUser User { get; set; }
    }
}