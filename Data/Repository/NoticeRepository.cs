

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class NoticeRepository : Repository<Notice>, INoticeRepository
    {
        private readonly ApplicationDbContext _db;
        public NoticeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Notice obj)
        {
            _db.notices.Update(obj);
            await SaveAsync();

        }


    }
}