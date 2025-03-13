

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly ApplicationDbContext _db;
        public FeedbackRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(Feedback obj)
        {
            _db.feedbacks.Update(obj);
            await SaveAsync();

        }


    }
}