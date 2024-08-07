using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class ExpenseService
  {
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
      _expenseRepository = expenseRepository;
    }

    public async Task<Guid> AddExpenseAsync(Expense expense)
    {
      return await _expenseRepository.AddExpenseAsync(expense);
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
      return await _expenseRepository.GetAllExpensesAsync();
    }

    public async Task<Expense> GetExpenseByIdAsync(Guid id)
    {
      return await _expenseRepository.GetExpenseByIdAsync(id);
    }

    public async Task UpdateExpenseAsync(Expense expense)
    {
      throw new NotImplementedException();
    }

    public async Task DeleteExpenseAsync(Guid id)
    {
      throw new NotImplementedException();
    }
  }
}
