using System.ComponentModel.DataAnnotations;

namespace your_auction_api.Models.Dto
{
    public class ForgetPassworedConfirmationDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public int Code { get; set; }
    }
}
