using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace your_auction_api.Models.Specifications
{
    public class UserSpecification : Specification
    {
        public string? role { get; set; }
        public string? status { get; set; }
    }
}