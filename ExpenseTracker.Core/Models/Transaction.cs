using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Models;

public class Transaction : IEntity
{
  private readonly Category? _category = null;
  private readonly Subcategory? _subcategory = null;

  public Guid Id { get; private set; }
  public string Description { get; private set; }
  public decimal Amount { get; private set; }
  public DateTime Date { get; private set; }
  public Guid CategoryId { get; private set; }
  public Guid SubcategoryId { get; private set; }
  public TransactionType TransactionType { get; private set; }
  public bool IsRecurrent { get; private set; }
  public bool IsDeleted { get; private set; }
  public DateTimeOffset? CreatedDateTime { get; private set; }
  public DateTimeOffset? UpdatedDateTime { get; private set; }

  public Category? Category => _category;
  public Subcategory? Subcategory => _subcategory;

  private Transaction()
  {

  }

  private Transaction(Guid id, string description, decimal amount, DateTime date, Guid categoryId, Guid subcategoryId, bool isRecurrent,
                      TransactionType transactionType, Category? category, Subcategory? subcategory, DateTimeOffset? createdDateTime = null, DateTimeOffset? updatedDateTime = null)
  {
    Id = id;
    Description = description;
    Amount = amount;
    Date = date;
    CategoryId = categoryId;
    SubcategoryId = subcategoryId;
    IsRecurrent = isRecurrent;
    TransactionType = transactionType;
    _category = category;
    _subcategory = subcategory;
    CreatedDateTime = createdDateTime;
    UpdatedDateTime = updatedDateTime;
  }

  public static Transaction CreateNew(string description, decimal amount, DateTime date, Guid categoryId, Guid subcategoryId,
                                      bool isRecurrent, TransactionType transactionType, Category? category = null, Subcategory? subcategory = null)
  {
    return new(
      Guid.NewGuid(),
      description,
      amount,
      date,
      categoryId,
      subcategoryId,
      isRecurrent,
      transactionType,
      category,
      subcategory
    );
  }

  public static Transaction Create(Guid id, string description, decimal amount, DateTime date, Guid categoryId, Guid subcategoryId,
                                   bool isRecurrent, TransactionType transactionType, Category? category = null, Subcategory? subcategory = null,
                                   DateTimeOffset? createdDateTime = null, DateTimeOffset? updatedDateTime = null)
  {
    return new(
      id,
      description,
      amount,
      date,
      categoryId,
      subcategoryId,
      isRecurrent,
      transactionType,
      category,
      subcategory,
      createdDateTime,
      updatedDateTime
    );
  }
}