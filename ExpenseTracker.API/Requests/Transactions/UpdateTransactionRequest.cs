namespace ExpenseTracker.API.Requests.Transactions;
public record UpdateTransactionRequest(Guid Id,
                                       string Description,
                                       decimal Amount,
                                       DateTime Date,
                                       Guid CategoryId,
                                       bool IsRecurrent,
                                       string TransactionType,
                                       int UserId);
