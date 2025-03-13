

using your_auction_api.Models;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IPaymentRepository
    {
        Task UpdateAsync(Payment obj);
    }
}