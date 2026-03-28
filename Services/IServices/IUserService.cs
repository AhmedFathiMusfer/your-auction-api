

using ErrorOr;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Services.IServices
{
    public interface IUserService
    {
        Task<ErrorOr<PaginatedResult<UserDTO>>> GetUsers(UserSpecification spec);
        Task<ErrorOr<ApplicationUser>> GetUserById(int userId);
        Task<ErrorOr<Success>> AddUser(UserDTO userDto);
        Task<ErrorOr<Success>> UpdateUser(int userId, UserDTO userDto);
        Task<ErrorOr<Deleted>> DeleteUser(int userId);
        Task<ErrorOr<int>> GetCountUsers();
    }
}