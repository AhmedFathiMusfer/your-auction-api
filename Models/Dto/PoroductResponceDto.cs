using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace your_auction_api.Models.Dto
{
    public class PoroductResponceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string sellerName { get; set; }

        public bool IsChecked { get; set; }
        public string? Emp_note { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> images { get; set; }
    }
}