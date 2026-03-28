

namespace your_auction_api.Models.Specifications
{
    public abstract class Specification
    {
        public int Id { get; set; }
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalCount { get; set; } = 0;
        public int TotalPages { get; set; } = 0;

        public string? Search { get; set; }
    }
}