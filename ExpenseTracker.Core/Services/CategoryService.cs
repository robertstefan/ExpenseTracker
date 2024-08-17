using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class CategoryService(ICategoriesRepository _categoryRepository)
{
    public async Task<Guid> AddCategoryAsync(Category category)
    {
        return await _categoryRepository.AddCategoryAsync(category);
    }

    public async Task<PaginatedResponse<Category>> GetCategoriesPaginatedAsync(int PageNumber, int PageSize)
    {
        return await _categoryRepository.GetCategoriesPaginatedAsync(PageNumber, PageSize);
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task<bool> UpdateCategoryAsync(Category expense)
    {
        return await _categoryRepository.UpdateCategoryAsync(expense);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id, bool SoftDelete)
    {
        return await _categoryRepository.DeleteCategoryAsync(id, SoftDelete);
    }
}
