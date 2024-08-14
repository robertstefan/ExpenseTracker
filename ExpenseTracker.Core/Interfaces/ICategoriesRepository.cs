using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<Guid> CreateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(Guid categoryId);
        Task<string> GetCategoryName(Guid categoryId);
    }
}
