namespace ExpenseTracker.API.DTOs
{
  public class TransactionDTO
  {
    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public bool IsRecurrent { get; set; }

    public string? Category { get; set; }

    public int TransactionType { get; set; }
  }
}
