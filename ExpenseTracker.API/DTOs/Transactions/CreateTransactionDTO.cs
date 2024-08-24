using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs.Transactions
{
    public class CreateTransactionDTO
    {
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public bool IsRecurrent { get; set; }
        public TransactionType TransactionType { get; set; }
        public int UserId { get; set; }
    }
}
