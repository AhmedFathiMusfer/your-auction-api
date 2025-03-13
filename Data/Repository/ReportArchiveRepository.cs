

using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;

namespace your_auction_api.Data.Repository
{
    public class ReportArchiveRepository : Repository<ReportArchive>, IReportArchiveRepository
    {
        private readonly ApplicationDbContext _db;
        public ReportArchiveRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task UpdateAsync(ReportArchive obj)
        {
            _db.reportArchives.Update(obj);
            await SaveAsync();

        }


    }
}