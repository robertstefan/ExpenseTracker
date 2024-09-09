namespace ExpenseTracker.Core.Models;

public class Transaction
{
  public Transaction()
  {
    Id = Guid.NewGuid();
  }

  public Guid Id { get; set; }
  public string Description { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public DateTime Date { get; set; }
  public bool IsRecurrent { get; set; }
  public int UserId { get; set; }

  public string Currency { get; set; }

  public double ExchangeRate { get; set; }

  public TransactionType Type { get; set; }

  public int CategoryId { get; set; }
  public Category Category { get; set; } = new(); // Navigation property

  public int SubcategoryId { get; set; }
  public Subcategory Subcategory { get; set; } = new(); // Navigation property
}