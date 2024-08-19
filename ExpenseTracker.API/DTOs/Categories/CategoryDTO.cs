using ExpenseTracker.API.DTOs.Subcategories;
using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Categories
{
    public class CategoryDTO
    {
        public Guid Id { get; set; } 

        public string CategoryName { get; set; } = string.Empty;

        public SubcategoryDetailsDTO Subcategory { get; set; }

        public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();

        public CategoryDTO()
        {

        }

        public CategoryDTO(Category category)
        {
            Id = category.Id;
            CategoryName = category.CategoryName;
            if (category.Subcategory != null) { Subcategory = new SubcategoryDetailsDTO(category.Subcategory); }

            foreach (var transaction in category.Transactions)
            {
                Transactions.Add(new TransactionDTO(transaction));
            }
        }
    }
}
