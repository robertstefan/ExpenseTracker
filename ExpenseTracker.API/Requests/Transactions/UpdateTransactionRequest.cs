using ExpenseTracker.API.Requests.Common;
using ExpenseTracker.Core.Common.Enums;

namespace ExpenseTracker.API.Requests.Transactions;
public record UpdateTransactionRequest(string Description,
                                       decimal Amount,
                                       DateTime Date,
                                       Guid CategoryId,
                                       Guid SubcategoryId,
                                       bool IsRecurrent,
                                       int TransactionType) : ValidationMiddleware
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

        if (SubcategoryId == Guid.Empty)
        {
            errors[nameof(CategoryId)] = "The subcategory cannot be empty";
        }

        if (!Enum.IsDefined(typeof(TransactionType), TransactionType))
        {
            errors[nameof(TransactionType)] = "This transaction type is not supported or wrong";
        }

        return errors;
    }
}
