using System.Text.Json.Serialization;

namespace ExpenseTracker.Core.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Expense,
        Income
    }
}
