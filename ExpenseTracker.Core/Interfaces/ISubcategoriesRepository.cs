using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ISubcategoriesRepository
    {
        Task<Subcategory> GetSubcategoryByIdAsync(Guid subcategoryId);
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
        Task<Guid> CreateSubcategoryAsync(Subcategory subcategory);
        Task<bool> DeleteSubcategoryAsync(Guid subcategoryId);
    }
}
