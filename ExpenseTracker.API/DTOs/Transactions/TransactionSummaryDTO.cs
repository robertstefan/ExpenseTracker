using ExpenseTracker.API.DTOs.Categories;
using ExpenseTracker.API.DTOs.Subcategories;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Transactions
{
    public class TransactionSummaryDTO
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool IsRecurrent { get; set; }

        public TransactionType TransactionType { get; set; }

        public CategoryDetailsDTO Category { get; set; }

        public SubcategoryDetailsDTO Subcategory { get; set; }

        public TransactionSummaryDTO()
        {

        }

        public TransactionSummaryDTO(Transaction transaction)
        {
            Id = transaction.Id;
            Description = transaction.Description;
            Amount = transaction.Amount;
            Date = transaction.Date;
            IsRecurrent = transaction.IsRecurrent;
            TransactionType = transaction.TransactionType;
            Category = new CategoryDetailsDTO(transaction.Category);
            Subcategory = new SubcategoryDetailsDTO(transaction.Subcategory);
        }
    }
}
