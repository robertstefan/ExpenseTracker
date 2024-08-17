using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class SubcategoryService(ISubcategoriesRepository _subcategoryRepository)
{
    public async Task<Guid> AddSubcategoryAsync(Subcategory category)
    {
        return await _subcategoryRepository.AddSubcategoryAsync(category);
    }

    public async Task<PaginatedResponse<Subcategory>> GetSubcategoriesPaginatedAsync(int PageNumber, int PageSize)
    {
        return await _subcategoryRepository.GetSubcategoriesPaginatedAsync(PageNumber, PageSize);
    }

    public async Task<Subcategory?> GetSubcategoryByIdAsync(Guid id)
    {
        return await _subcategoryRepository.GetSubcategoryByIdAsync(id);
    }

    public async Task<bool> UpdateSubcategoryAsync(Subcategory subcategory)
    {
        return await _subcategoryRepository.UpdateSubcategoryAsync(subcategory);
    }

    public async Task<bool> DeleteSubcategoryAsync(Guid id, bool SoftDelete)
    {
        return await _subcategoryRepository.DeleteSubcategoryAsync(id, SoftDelete);
    }
}
