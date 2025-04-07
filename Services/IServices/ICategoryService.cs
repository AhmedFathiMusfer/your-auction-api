

using ErrorOr;
using your_auction_api.Models;

namespace your_auction_api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ErrorOr<List<Category>>> GetCategories();
        Task<ErrorOr<Category>> getCategoryById(int categoryId);
        Task<ErrorOr<Success>> AddCategory(Category category);

        Task<ErrorOr<Success>> UpdateCategory(int categoryId, Category category);

        Task<ErrorOr<Deleted>> DeleteCategory(int categoryId);
    }
}