using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace your_auction_api.Models.Dto
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PaginatedResult(List<T> items, int count, int page, int pageSize)
        {
            Items = items;
            TotalItems = count;
            Page = page;
            PageSize = pageSize;
        }
    }
}