using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Validation;

namespace ExpenseTracker.API.DTOs
{
  // @TODO: move validations here??
  public class TransactionDTO
  {
    // @TODO: nullable?
    public string? Description { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public bool IsRecurrent { get; set; }


    // using attribute to validate so i don't have to do it everywhere i create a model/take user input
    [ValidTransactionType]
    public TransactionType Type { get; set; }

    public int CategoryId { get; set; }
    public int SubcategoryId { get; set; }

  }
}
