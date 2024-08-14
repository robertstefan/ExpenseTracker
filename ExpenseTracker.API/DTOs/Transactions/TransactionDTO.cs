using ExpenseTracker.Core.Models;
using System.Text.Json.Serialization;

namespace ExpenseTracker.API.DTOs.Transactions
{
    public class TransactionDTO
    {
        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool IsRecurrent { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        public string CategoryName { get; set; } = string.Empty;
    }
}
