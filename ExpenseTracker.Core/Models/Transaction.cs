namespace ExpenseTracker.Core.Models
{
  public class Transaction
  {
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public bool IsRecurrent { get; set; }

    public TransactionType Type { get; set; } 

    public int CategoryId { get; set; }
    // Navigation property
    public Category Category { get; set; } = new Category();

    public Transaction()
    {
      Id = Guid.NewGuid();
    }
  }
}
