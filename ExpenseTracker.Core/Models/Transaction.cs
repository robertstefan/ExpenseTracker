using System.ComponentModel.DataAnnotations.Schema;
using ExpenseTracker.Core.Common.Enums;

namespace ExpenseTracker.Core.Models;

public class Transaction
{
  public Guid Id { get; private set; }
  public string Description { get; private set; }
  public decimal Amount { get; private set; }
  public DateTime Date { get; private set; }
  public Guid CategoryId { get; private set; }
  public bool IsRecurrent { get; private set; }
  public TransactionType TransactionType { get; private set; }
  public DateTimeOffset CreatedDateTime { get; private set; }
  public DateTimeOffset UpdatedDateTime { get; private set; }
  public bool IsDeleted { get; private set; }

  [NotMapped]
  public string CategoryName { get; set; }

  private Transaction()
  {

  }

  private Transaction(Guid id, string description, decimal amount, DateTime date, Guid categoryId, bool isRecurrent,
                      TransactionType transactionType)
  {
    Id = id;
    Description = description;
    Amount = amount;
    Date = date;
    CategoryId = categoryId;
    IsRecurrent = isRecurrent;
    TransactionType = transactionType;
  }

  public static Transaction CreateNew(string description, decimal amount, DateTime date, Guid categoryId,
                                      bool isRecurrent, TransactionType transactionType)
  {
    return new(
      Guid.NewGuid(),
      description,
      amount,
      date,
      categoryId,
      isRecurrent,
      transactionType
    );
  }

  public static Transaction Create(Guid id, string description, decimal amount, DateTime date, Guid categoryId,
                                   bool isRecurrent, TransactionType transactionType)
  {
    return new(
      id,
      description,
      amount,
      date,
      categoryId,
      isRecurrent,
      transactionType
    );
  }
}