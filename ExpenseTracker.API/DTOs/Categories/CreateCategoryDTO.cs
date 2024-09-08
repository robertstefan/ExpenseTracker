using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Categories
{
    public class CreateCategoryDTO
    {
        [Required]
        public string CategoryName { get; set; }
    }
}
