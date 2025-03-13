

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(ApplicationUser obj)
        {
            _db.users.Update(obj);
            await SaveAsync();

        }


    }
}