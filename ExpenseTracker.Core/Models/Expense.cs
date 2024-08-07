namespace ExpenseTracker.Core.Models
{
  public class Expense
  {
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; }
  }
}
