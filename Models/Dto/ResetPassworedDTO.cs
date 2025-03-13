using System.ComponentModel.DataAnnotations;

namespace your_auction_api.Models.Dto
{
    public class ResetPassworedDTO
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
        public int Code { get; set; }
    }
}
