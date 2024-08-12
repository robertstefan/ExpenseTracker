using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class CategoryService(ICategoriesRepository _categoryRepository)
{
    public async Task<Guid> AddCategoryAsync(Category category)
    {
        return await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task<IEnumerable<Category>> GetCategoriesPaginatedAsync(int offset, int limit)
    {
        return await _categoryRepository.GetCategoriesPaginatedAsync(offset, limit);
    }

    public async Task<Category> GetCategoryByIdAsync(string id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task UpdateCategoryAsync(Category expense)
    {
        await _categoryRepository.UpdateCategoryAsync(expense);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        await _categoryRepository.DeleteCategoryAsync(id);
    }
}
