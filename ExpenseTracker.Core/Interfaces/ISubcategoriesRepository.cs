using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ISubcategoriesRepository
{
    Task<Guid> AddSubcategoryAsync(Subcategory Subcategory);
    Task<bool> DeleteSubcategoryAsync(Guid id, bool SoftDelete);
    Task<PaginatedResponse<Subcategory>> GetSubcategoriesPaginatedAsync(int PageNumber, int PageSize);
    Task<Subcategory?> GetSubcategoryByIdAsync(Guid id);
    Task<bool> UpdateSubcategoryAsync(Subcategory Subcategory);
}
