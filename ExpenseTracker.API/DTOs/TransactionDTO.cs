using ExpenseTracker.API.Common.Interfaces;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs;

public class TransactionDTO : IEntityDTO
{
  public Guid Id { get; set; }
  public string? Description { get; set; }
  public DateTime Date { get; set; }
  public decimal Amount { get; set; }
  public bool IsRecurrent { get; set; }
  public string TransactionType { get; set; }
  public CategoryDTO? Category { get; set; }
  public DateTimeOffset? CreatedDateTime { get; set; }
  public DateTimeOffset? UpdatedDateTime { get; set; }

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
    TransactionType = transaction.TransactionType.ToString();
    CreatedDateTime = transaction.CreatedDateTime;
    UpdatedDateTime = transaction.UpdatedDateTime;
    Category = new CategoryDTO(transaction.Category, transaction.CategoryId);
  }

}

