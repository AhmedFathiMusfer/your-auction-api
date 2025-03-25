

using System.ComponentModel.DataAnnotations.Schema;

namespace your_auction_api.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public Product Product { get; set; }
    }
}