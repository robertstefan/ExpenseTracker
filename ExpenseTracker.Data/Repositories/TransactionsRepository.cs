using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
  public class TransactionsRepository : ITransactionRepository
  {
    private readonly string _connectionString;
    private string TableName => "[Transactions]";

    public TransactionsRepository(string connectionString)
    {
      _connectionString = connectionString;
    }

    public async Task<Guid> AddTransactionAsync(Transaction transaction)
    {
      using var connection = new SqlConnection(_connectionString);
      var query = $@"INSERT INTO {TableName} 
                              (Id, Description, Amount, Date, Category, IsRecurrent, TransactionType)
                          VALUES (@Id, @Description,@Amount, @Date, @Category, @IsRecurrent, @TypeId)";

      await connection.ExecuteAsync(query, new
      {
        transaction.Id,
        transaction.Description,
        transaction.Amount,
        transaction.Date,
        transaction.Category,
        transaction.IsRecurrent,
        TypeId = (int)transaction.Type // Convert enum to int
      });

      return transaction.Id;
    }

    public async Task<bool> DeleteTransactionAsync(Guid transactionId)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $"DELETE FROM {TableName} WHERE Id = @Id; SELECT @@ROWCOUNT AS Affected";

      var affectedRows = await conn.ExecuteScalarAsync<int>(query, new { Id = transactionId });

      return affectedRows == 1;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $@"SELECT 
                      Id, 
                      Description, 
                      Amount, 
                      Date, 
                      Category, 
                      IsRecurrent, 
                      TransactionType AS Type 
                  FROM {TableName}";

      return await conn.QueryAsync<Transaction>(query);
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $@"SELECT 
                      Id, 
                      Description, 
                      Amount, 
                      Date, 
                      Category, 
                      IsRecurrent, 
                      TransactionType AS Type 
                  FROM {TableName} 
                  WHERE Id = @Id";
      return await conn.QuerySingleAsync<Transaction>(query, new { Id = transactionId });
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType type)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = @$"SELECT 
                      Id, 
                      Description, 
                      Amount, 
                      Date, 
                      Category, 
                      IsRecurrent, 
                      TransactionType AS Type 
                  FROM {TableName} 
                  WHERE TransactionType = @TypeId";

      return await conn.QueryAsync<Transaction>(query, new { TypeId = (int)type });
    }

    public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $@"UPDATE {TableName}
                              SET 
                                  Description = @Description,
                                  Amount = @Amount,
                                  Date = @Date,
                                  Category = @Category,
                                  IsRecurrent = @IsRecurrent,
                                  TransactionType = @TypeId
                              
                             WHERE Id = @Id";

      var result = await conn.ExecuteAsync(query, new
      {
        transaction.Id,
        transaction.Description,
        transaction.Amount,
        transaction.Date,
        transaction.Category,
        transaction.IsRecurrent,
        TypeId = (int)transaction.Type // Convert enum to int
      });

      if (result == 0)
      {
        return null;
      }

      return transaction;
    }
  }
}
