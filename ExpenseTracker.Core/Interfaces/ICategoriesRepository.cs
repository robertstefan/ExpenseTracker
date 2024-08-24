using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        //Task<IEnumerable<Category>> GetSubcategoriesByParentCategoryIdAsync(Guid parentCategoryId);
        Task<Guid> CreateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
    }
}
