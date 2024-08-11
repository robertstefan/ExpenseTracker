using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs;

public class TransactionDTO
{
  public Guid Id { get; set; }
  public string? Description { get; set; }

  public DateTime Date { get; set; }

  public decimal Amount { get; set; }

  public bool IsRecurrent { get; set; }

  public string CategoryName { get; set; }

  public string TransactionType { get; set; }

  public DateTimeOffset CreatedDateTime { get; set; }
  public DateTimeOffset UpdatedDateTime { get; set; }

  public TransactionDTO()
  {

  }

  public TransactionDTO(Transaction transaction)
  {
    Id = transaction.Id;
    Description = transaction.Description;
    Date = transaction.Date;
    Amount = transaction.Amount;
    IsRecurrent = transaction.IsRecurrent;
    CategoryName = transaction.CategoryName;
    TransactionType = transaction.TransactionType.ToString();
    CreatedDateTime = transaction.CreatedDateTime;
    UpdatedDateTime = transaction.UpdatedDateTime;
  }

}

