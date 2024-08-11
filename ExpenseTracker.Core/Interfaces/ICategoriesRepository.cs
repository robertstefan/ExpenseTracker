using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ICategoriesRepository
{
    Task<Guid> AddCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(string id);
    Task<IEnumerable<Category>> GetCategoriesPaginatedAsync(int offset, int limit);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Guid id);
}
