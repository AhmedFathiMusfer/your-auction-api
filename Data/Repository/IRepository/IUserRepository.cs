

using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
        Task UpdateAsync(ApplicationUser obj);
        Task<PaginatedResult<UserDTO>> GetAll(UserSpecification spec);
    }
}