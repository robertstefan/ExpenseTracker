using System.Text.Json.Serialization;

namespace ExpenseTracker.Core.Models
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool IsRecurrent { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType TransactionType { get; set; }

        public Guid CategoryId { get; set; }
        public int UserId { get; set; }
        public Category? Category { get; set; }

    }
}
