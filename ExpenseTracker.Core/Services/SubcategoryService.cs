using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class SubcategoryService
  {
    private readonly ISubcategoryRepository _subcategoryRepository;

    public SubcategoryService(ISubcategoryRepository subcategoryRepository)
    {
      _subcategoryRepository = subcategoryRepository;
    }

    public async Task<int> AddSubcategoryAsync(Subcategory subcategory)
    {
      return await _subcategoryRepository.AddSubcategoryAsync(subcategory);
    }

    public async Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId)
    {
      return await _subcategoryRepository.GetSubcategoryByIdAsync(subcategoryId);
    }

    public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
    {
      return await _subcategoryRepository.GetAllSubcategoriesAsync();
    }

    public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId)
    {
      return await _subcategoryRepository.GetSubcategoriesByCategoryIdAsync(categoryId);
    }

    public async Task<Subcategory?> UpdateSubcategoryAsync(Subcategory subcategory)
    {
      return await _subcategoryRepository.UpdateSubcategoryAsync(subcategory);
    }

    public async Task<bool> DeleteSubcategoryAsync(int subcategoryId)
    {
      return await _subcategoryRepository.DeleteSubcategoryAsync(subcategoryId);
    }
  }
}
