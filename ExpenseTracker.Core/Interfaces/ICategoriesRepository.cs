using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ICategoriesRepository
{
    Task<Guid> AddCategoryAsync(Category category);
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task<PaginatedResponse<Category>> GetCategoriesPaginatedAsync(int PageNumber, int PageSize);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(Guid id, bool SoftDelete);
}
