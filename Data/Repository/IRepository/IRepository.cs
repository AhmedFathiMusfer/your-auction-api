
using System.Linq.Expressions;

namespace your_auction_api.Data.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool Tracked = true, string? includeProperties = null);
        Task CreateAsync(T obj);

        Task RemoveAsync(T obj);
        Task SaveAsync();
    }
}