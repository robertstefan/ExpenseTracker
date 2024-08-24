
using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Categories
{
    public class CategoryDTO
    {
        public Guid Id { get; set; } 

        public string CategoryName { get; set; } = string.Empty;

        public List<TransactionDTO> Transactions { get; set; } = new List<TransactionDTO>();

        public Guid? ParentCategoryId { get; set; }

        public CategoryDTO()
        {

        }

        public CategoryDTO(Category category)
        {
            Id = category.Id;
            CategoryName = category.CategoryName;
            ParentCategoryId = category.ParentCategoryId;

            foreach (var transaction in category.Transactions)
            {
                Transactions.Add(new TransactionDTO(transaction));
            }
        }
    }
}
