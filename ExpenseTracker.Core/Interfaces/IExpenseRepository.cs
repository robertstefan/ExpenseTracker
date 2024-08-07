using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
  public interface IExpenseRepository
  {
    Task<Guid> AddExpenseAsync(Expense expense);
    Task<IEnumerable<Expense>> GetAllExpensesAsync();
    Task<Expense> GetExpenseByIdAsync(Guid id);

    //Task UpdateExpenseAsync(Expense expense);
    //Task DeleteExpenseAsync(Guid id);
  }
}
