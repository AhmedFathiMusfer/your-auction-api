
using Microsoft.AspNetCore.Identity;
using your_auction_api.Utility;

namespace your_auction_api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string ProfilePictureUrl { get; set; }

        public UserStatus status { get; set; } = UserStatus.InVerification;
        public string? Address { get; set; }

    }
}