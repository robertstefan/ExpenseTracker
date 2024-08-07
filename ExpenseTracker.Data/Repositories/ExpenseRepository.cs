using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
  public class ExpenseRepository : IExpenseRepository
  {
    private readonly string _connectionString;

    public ExpenseRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public async Task<Guid> AddExpenseAsync(Expense expense)
    {
      using var connection = new SqlConnection(_connectionString);
      var sql = @"INSERT INTO Expenses (Id, Description, Amount, Date, Category)
                        VALUES (@Id, @Description, @Amount, @Date, @Category)";

      await connection.ExecuteAsync(sql, expense);

      return expense.Id;
    }

    public Task DeleteExpenseAsync(Guid id)
    {
      throw new NotImplementedException();
    }

    public async Task<IEnumerable<Expense>> GetAllExpensesAsync()
    {
      using var connection = new SqlConnection(_connectionString);

      return await connection.QueryAsync<Expense>("SELECT * FROM Expenses");
    }

    public async Task<Expense> GetExpenseByIdAsync(Guid id)
    {
      using var connection = new SqlConnection(_connectionString);

      return await connection.QueryFirstAsync<Expense>("SELECT * FROM Expenses WHERE Id = @Id", new { Id = id });
    }

    public Task UpdateExpenseAsync(Expense expense)
    {
      throw new NotImplementedException();
    }
  }
}
