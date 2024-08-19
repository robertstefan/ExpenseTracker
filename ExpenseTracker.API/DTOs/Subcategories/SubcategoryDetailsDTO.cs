using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Subcategories
{
    public class SubcategoryDetailsDTO
    {
        public Guid Id { get; set; } 

        public string SubcategoryName { get; set; } = string.Empty;

        public SubcategoryDetailsDTO()
        {

        }

        public SubcategoryDetailsDTO(Subcategory subcategory)
        {
            Id = subcategory.Id;
            SubcategoryName = subcategory.SubcategoryName;
        }
    }
}
