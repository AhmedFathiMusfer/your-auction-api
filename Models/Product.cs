

using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace your_auction_api.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public bool IsChecked { get; set; }
        public string Emp_note { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
        public ApplicationUser User { get; set; }
        [JsonIgnore]
        public ICollection<ProductImage> ProductImages { get; set; }

        [NotMapped]
        public List<string> images { get; set; }
    }

}