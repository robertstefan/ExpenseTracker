namespace ExpenseTracker.API.Requests.Transactions;
public record CreateTransactionRequest(string Description,
                                       decimal Amount,
                                       DateTime Date,
                                       Guid CategoryId,
                                       bool IsRecurrent,
                                       string TransactionType,
                                       int UserId,  
                                       string currency,
                                       double exchangeRate);

//public class CreateTransactionRequest
//{
//  public string Description { get; set; }
//  public decimal Amount { get; set; }
//  public DateTime Date { get; set; }
//  public Guid CategoryId { get; set; }
//  public bool IsRecurrent { get; set; }
//  public TransactionType TransactionType { get; set; }

//  public CreateTransactionRequest(string description, decimal amount, DateTime date, Guid categoryId, bool isRecurrent, int? transactionType)
//  {
//    Description = description;
//    Amount = amount;
//    Date = date;
//    CategoryId = categoryId;
//    IsRecurrent = isRecurrent;

//    TransactionType _ttype;
//    if (transactionType.HasValue)
//    {
//      Enum.TryParse<TransactionType>(transactionType.Value.ToString(), out _ttype);
//      TransactionType = _ttype;
//    }
//    else
//      TransactionType = TransactionType.Expense;
//  }
//}
