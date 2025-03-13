

using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface INoticeRepository
    {
        Task UpdateAsync(Notice obj);
    }
}