using ExpenseTracker.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Transactions
{
    public class CreateTransactionDTO
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public bool IsRecurrent { get; set; }

        [Required]
        public TransactionType TransactionType { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
