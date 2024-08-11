namespace ExpenseTracker.API.Requests.Transactions;
public record CreateTransactionRequest(string Description,
                                       decimal Amount,
                                       DateTime Date,
                                       Guid CategoryId,
                                       bool IsRecurrent,
                                       string TransactionType);
