using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Validation;

namespace ExpenseTracker.API.DTOs;

// @TODO: move validations here??
public class TransactionDTO
{
  public string? Description { get; set; }

  public DateTime Date { get; set; }

  public decimal Amount { get; set; }

  public bool IsRecurrent { get; set; }


  // using attribute to validate, so I don't have to do it everywhere I create a model/take user input
  [ValidTransactionType]
  public TransactionType Type { get; set; }

  public int CategoryId { get; set; }
  public int SubcategoryId { get; set; }
  public int UserId { get; set; }
}