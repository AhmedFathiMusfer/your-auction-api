
using AutoMapper;
using ErrorOr;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<ErrorOr<PaginatedResult<UserDTO>>> GetUsers(UserSpecification spec)
        {
            var users = await _userRepository.GetAll(spec);
            return users;

        }
        public Task<ErrorOr<ApplicationUser>> GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<Success>> AddUser(UserDTO userDto)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<Success>> UpdateUser(int userId, UserDTO userDto)
        {
            throw new NotImplementedException();
        }

        public Task<ErrorOr<Deleted>> DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ErrorOr<int>> GetCountUsers()
        {
            var count = await _userRepository.GetCountAsync();
            return count;
        }

    }
}