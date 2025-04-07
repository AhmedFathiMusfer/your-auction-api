

using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using your_auction_api.Data.Repository.IRepository;
using your_auction_api.Models;
using your_auction_api.Services.IServices;

namespace your_auction_api.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<Category> _categoryValidator;

        public CategoryService(ICategoryRepository categoryRepository, IValidator<Category> categoryValidator)
        {
            _categoryRepository = categoryRepository;
            _categoryValidator = categoryValidator;
        }

        public async Task<ErrorOr<Success>> AddCategory(Category category)
        {
            var resultValidator = _categoryValidator.Validate(category);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll((error) => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();

                return error;
            }
            await _categoryRepository.CreateAsync(category);
            return Result.Success;
        }

        public async Task<ErrorOr<Deleted>> DeleteCategory(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == categoryId, Tracked: false);
            if (category is null)
            {
                return Error.NotFound(description: "this category not found");
            }
            await _categoryRepository.RemoveAsync(category);
            return Result.Deleted;
        }

        public async Task<ErrorOr<List<Category>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories;
        }

        public async Task<ErrorOr<Category>> getCategoryById(int categoryId)
        {
            var category = await _categoryRepository.GetAsync(c => c.Id == categoryId);
            if (category is null)
            {
                return Error.NotFound(description: "this category not found");
            }
            return category;
        }

        public async Task<ErrorOr<Success>> UpdateCategory(int categoryId, Category category)
        {
            var resultValidator = _categoryValidator.Validate(category);
            if (!resultValidator.IsValid)
            {
                var error = resultValidator.Errors.ConvertAll((error) => Error.Validation(code: error.PropertyName, description: error.ErrorMessage)).ToList();

                return error;
            }
            var oldCategory = await _categoryRepository.GetAsync(c => c.Id == categoryId, Tracked: false);

            if (category is null || categoryId != category.Id)
            {
                return Error.NotFound(description: "this category not found");
            }
            await _categoryRepository.UpdateAsync(category);
            return Result.Success;
        }
    }
}