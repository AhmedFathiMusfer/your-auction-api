
using Microsoft.AspNetCore.Identity;

namespace your_auction_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

    }
}