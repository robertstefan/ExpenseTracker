namespace ExpenseTracker.Core.Common.Enums
{
  public enum TransactionType
  {
    Income = 1,     // IN
    Refund = 2,     // IN
    Deposit = 3,     // IN

    Expense = 6,     // OUT
    Transfer = 7,   // OUT
    Investment = 8, // OUT
    Withdrawal = 9 // OUT
  }
}