using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs;

public class TransactionDTO
{
  public Guid Id { get; set; }
  public string? Description { get; set; }

  public DateTime Date { get; set; }

  public decimal Amount { get; set; }

  public bool IsRecurrent { get; set; }

  public Guid CategoryId { get; set; }

  public string TransactionType { get; set; }

  public DateTimeOffset CreatedDateTime { get; set; }
  public DateTimeOffset UpdatedDateTime { get; set; }

  public string Currency { get; set; }
  public double ExchangeRate { get; set; }

  public TransactionDTO()
  {

  }

  public TransactionDTO(Transaction transaction)
  {
    Id = transaction.Id;
    Description = transaction.Description;
    Date = transaction.Date;
    Amount = transaction.Currency != "RON" ? (decimal)transaction.ExchangeRate * transaction.Amount : transaction.Amount;
    IsRecurrent = transaction.IsRecurrent;
    CategoryId = transaction.CategoryId;
    TransactionType = transaction.TransactionType.ToString();
    CreatedDateTime = transaction.CreatedDateTime;
    UpdatedDateTime = transaction.UpdatedDateTime;
    Currency = transaction.Currency;
    ExchangeRate = transaction.ExchangeRate;
  }

}

