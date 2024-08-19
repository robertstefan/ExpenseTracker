using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class SubcategoriesService
    {
        private readonly ISubcategoriesRepository _subcategoriesRepository;

        public SubcategoriesService(ISubcategoriesRepository subcategoriesRepository)
        {
            _subcategoriesRepository = subcategoriesRepository;
        }


        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
        {
            return await _subcategoriesRepository.GetAllSubcategoriesAsync();
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid subcategoryId)
        {
            return await _subcategoriesRepository.GetSubcategoryByIdAsync(subcategoryId);
        }
        public async Task<Guid> CreateSubcategoryAsync(Subcategory subcategory)
        {
            return await _subcategoriesRepository.CreateSubcategoryAsync(subcategory);
        }

        public async Task<bool> DeleteSubcategoryAsync(Guid subcategoryId)
        {
            return await _subcategoriesRepository.DeleteSubcategoryAsync(subcategoryId);
        }


    }
}
