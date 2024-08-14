using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class CategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<Guid> CreateCategoryAsync(Category category)
        {
            return await _categoriesRepository.CreateCategoryAsync(category);
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            return await _categoriesRepository.DeleteCategoryAsync(categoryId);
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            return await _categoriesRepository.GetCategoryByIdAsync(categoryId);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoriesRepository.GetAllCategoriesAsync();
        }

        public async Task<string> GetCategoryName(Guid categoryId)
        {
            return await _categoriesRepository.GetCategoryName(categoryId);
        }
    }
}
