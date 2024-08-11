using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
  public interface ICategoryRepository
  {
    Task<int> AddCategoryAsync(Category category);

    Task<Category> GetCategoryByIdAsync(int categoryId);

    Task<IEnumerable<Category>> GetAllCategoriesAsync();

    Task<Category?> UpdateCategoryAsync(Category category);

    Task<bool> DeleteCategoryAsync(int categoryId);

  }
}
