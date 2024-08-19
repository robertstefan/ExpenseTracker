using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Categories
{
    public class CategoryDetailsDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CategoryName { get; set; } = string.Empty;

        public CategoryDetailsDTO()
        {

        }

        public CategoryDetailsDTO(Category category)
        {
            Id = category.Id;
            CategoryName = category.CategoryName;
        }
    }
}
