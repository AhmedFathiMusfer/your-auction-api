using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace your_auction_api.Models.Specifications
{
    public class ProductSpecification : Specification
    {
        public string? Category { get; set; }
    }
}