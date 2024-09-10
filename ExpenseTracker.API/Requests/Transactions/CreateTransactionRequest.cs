using System.Text.RegularExpressions;
using ExpenseTracker.API.Requests.Common;
using ExpenseTracker.Core.Common.Enums;

namespace ExpenseTracker.API.Requests.Transactions;
public partial record CreateTransactionRequest(
    string Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId,
    bool IsRecurrent,
    int TransactionType,
    Guid UserId,
    string Currency,
    double ExchangeRate) : ValidationMiddleware
{
    protected override Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (this is null)
        {
            errors[nameof(CreateTransactionRequest)] = "The request could not be null";
        }

        if (Amount <= 0)
        {
            errors[nameof(Amount)] = "Amount cannot be equal or less than zero.";
        }

        if (Amount > int.MaxValue)
        {
            errors[nameof(Amount)] = $"Amount cannot be bigger than {int.MaxValue}.";
        }

        if (Date <= DateTime.Now.AddYears(-1))
        {
            errors[nameof(Date)] = "The date cannot be from last year.";
        }

        if (Date > DateTime.Now)
        {
            errors[nameof(Date)] = "The date cannot be in the future.";
        }

        if (CategoryId == Guid.Empty)
        {
            errors[nameof(CategoryId)] = "The category cannot be empty";
        }

        if (IntToEnum.Handle<TransactionType>(TransactionType) == null)
        {
            errors[nameof(TransactionType)] = "This transaction type is not supported or wrong";
        }

        if (!LettersOnly().IsMatch(Currency))
        {
            errors[nameof(TransactionType)] = "No special characters allowed !";
        }

        return errors;
    }

    [GeneratedRegex("^[a-zA-Z]+$")]
    private static partial Regex LettersOnly();
}
