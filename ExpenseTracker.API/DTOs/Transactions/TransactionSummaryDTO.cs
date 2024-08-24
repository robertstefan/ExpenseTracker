using ExpenseTracker.API.DTOs.Categories;

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
        public CategoryDetailsDTO? ParentCategory { get; set; }

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
            if (transaction.Category.ParentCategory != null)
            {
                ParentCategory = new CategoryDetailsDTO(transaction.Category.ParentCategory);
            }
        }
    }
}
