using System.ComponentModel.DataAnnotations;

namespace your_auction_api.Models.Dto
{
    public class ForgetPassworedDTO
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
