

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class CommissionRepository : Repository<Commission>, ICommissionRepository
    {
        private readonly ApplicationDbContext _db;
        public CommissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Commission obj)
        {
            _db.commissions.Update(obj);
            await SaveAsync();

        }


    }
}