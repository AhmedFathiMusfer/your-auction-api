

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using your_auction_api.Utility;

namespace your_auction_api.Models
{
    public class Auction
    {
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public AuctionState state { get; set; }

        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        [JsonIgnore]
        public ICollection<AuctionUser> Users { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}