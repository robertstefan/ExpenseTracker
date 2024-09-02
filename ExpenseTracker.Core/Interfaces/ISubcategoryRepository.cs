using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ISubcategoryRepository
{
  Task<int> AddSubcategoryAsync(Subcategory subcategory);
  Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId);
  Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync();
  Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId);
  Task<Subcategory?> UpdateSubcategoryAsync(Subcategory subcategory);
  Task<bool> DeleteSubcategoryAsync(int subcategoryId);
}