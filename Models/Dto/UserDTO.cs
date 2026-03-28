using your_auction_api.Utility;

namespace your_auction_api.Models.Dto
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
        public UserStatus status { get; set; }
        public string profilePictureUrl { get; set; }
        public string? phoneNumber { get; set; }

    }
}
