using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Validation;

public class ValidTransactionType : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    var intValue = (int)value;
    if (Enum.IsDefined(typeof(TransactionType), intValue)) return ValidationResult.Success!;
    return new ValidationResult("Invalid transaction type.");
  }
}