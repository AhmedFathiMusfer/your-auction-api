

using Microsoft.EntityFrameworkCore;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Models.Dto;
using your_auction_api.Models.Specifications;

namespace your_auction_api.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<PaginatedResult<UserDTO>> GetAll(UserSpecification spec)
        {
            var users = await ApplySpecification(spec);
            var userDetails = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                phoneNumber = u.PhoneNumber,
                profilePictureUrl = u.ProfilePictureUrl,
                Address = u.Address,
                status = u.status
            }).ToListAsync();
            var result = new PaginatedResult<UserDTO>(
                 await userDetails,
                  spec.TotalCount,
                  spec.PageNumber,
                  spec.PageSize
              );
            return result;
        }

        public async Task UpdateAsync(ApplicationUser obj)
        {
            _db.users.Update(obj);
            await SaveAsync();

        }
        private async Task<IQueryable<ApplicationUser>> ApplySpecification(UserSpecification spec)
        {
            IQueryable<ApplicationUser> users = _db.users;
            if (!string.IsNullOrEmpty(spec.Search))
            {
                users = users.Where(a => a.Name.ToLower().Contains(spec.Search.ToLower()));
            }
            if (spec.PageNumber > 0 && spec.PageSize > 0)
            {
                spec.TotalCount = await users.CountAsync();
                if (spec.PageSize > 100)
                {
                    spec.PageSize = 100;
                }
            }
            else
            {
                spec.PageNumber = 1;
                spec.PageSize = 10;

            }

            if (string.IsNullOrEmpty(spec.OrderBy))
            {
                users = users.OrderByDescending(a => a.Id);
            }
            else
            {
                if (spec.OrderDirection == "desc")
                {
                    users = users.OrderByDescending(a => EF.Property<object>(a, spec.OrderBy));
                }
                else
                {
                    users = users.OrderBy(a => EF.Property<object>(a, spec.OrderBy));
                }
            }

            users = users.Skip(spec.PageSize * (spec.PageNumber - 1)).Take(spec.PageSize);
            return users;
        }





    }
}