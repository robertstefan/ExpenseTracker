using ExpenseTracker.Core.Models;
using System.ComponentModel.DataAnnotations;


namespace ExpenseTracker.Core.Validation
{
  public class ValidTransactionType : ValidationAttribute
  {
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is int intValue && Enum.IsDefined(typeof(TransactionType), intValue))
      {
        return ValidationResult.Success!;
      }

      return new ValidationResult("Invalid transaction type.");
    }
  }
}
