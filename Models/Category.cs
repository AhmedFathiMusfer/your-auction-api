

using System.Text.Json.Serialization;

namespace your_auction_api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }
    }
}