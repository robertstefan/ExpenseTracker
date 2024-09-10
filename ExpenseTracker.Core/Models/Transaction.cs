using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Models;

public class Transaction : IEntity
{
  private readonly Category? _category = null;
  public Guid Id { get; private set; }
  public string Description { get; private set; }
  public decimal Amount { get; private set; }
  public DateTime Date { get; private set; }
  public Guid CategoryId { get; private set; }
  public TransactionType TransactionType { get; private set; }
  public bool IsRecurrent { get; private set; }
  public bool IsDeleted { get; private set; }
  public Guid UserId { get; private set; }
  public string Currency { get; private set; }
  public double ExchangeRate { get; set; }
  public DateTimeOffset? CreatedDateTime { get; private set; }
  public DateTimeOffset? UpdatedDateTime { get; private set; }

  public Category? Category => _category;

  private Transaction()
  {

  }

  private Transaction(
      Guid id,
      string description,
      decimal amount,
      DateTime date,
      Guid categoryId,
      bool isRecurrent,
      TransactionType transactionType,
      Guid userId,
      Category? category,
      string currency = "RON",
      double exchangeRate = 1.0d,
      DateTimeOffset? createdDateTime = null,
      DateTimeOffset? updatedDateTime = null)
  {
    Id = id;
    Description = description;
    Amount = amount;
    Date = date;
    CategoryId = categoryId;
    IsRecurrent = isRecurrent;
    TransactionType = transactionType;
    UserId = userId;
    _category = category;
    Currency = currency;
    ExchangeRate = exchangeRate;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

  public static Transaction CreateNew(
      string description,
      decimal amount,
      DateTime date,
      Guid categoryId,
      bool isRecurrent,
      TransactionType transactionType,
      Guid userId,
      string currency = "RON",
      double exchangeRate = 1.0d,
      Category? category = null)
  {
    return new(
      Guid.NewGuid(),
      description,
      amount,
      date,
      categoryId,
      isRecurrent,
      transactionType,
      userId,
      category,
      currency,
      exchangeRate
    );
  }

  public static Transaction Create(
      Guid id,
      string description,
      decimal amount,
      DateTime date,
      Guid categoryId,
      bool isRecurrent,
      TransactionType transactionType,
      Guid userId,
      Category? category = null,
      string currency = "RON",
      double exchangeRate = 1.0d,
      DateTimeOffset? createdDateTime = null,
      DateTimeOffset? updatedDateTime = null)
  {
    return new(
      id,
      description,
      amount,
      date,
      categoryId,
      isRecurrent,
      transactionType,
      userId,
      category,
      currency,
      exchangeRate,
      createdDateTime,
      updatedDateTime
    );
  }
}