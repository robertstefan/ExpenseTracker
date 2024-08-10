namespace ExpenseTracker.Core.Models
{
  public class Transaction
  {
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;

    public bool IsRecurrent { get; set; }

    public TransactionType Type { get; set; }

    public Transaction()
    {
      Id = Guid.NewGuid();
    }
  }
}
