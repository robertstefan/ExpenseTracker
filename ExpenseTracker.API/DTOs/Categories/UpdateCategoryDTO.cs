using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Categories
{
    public class UpdateCategoryDTO
    {
        [Required]
        public string CategoryName { get; set; } 
    }
}