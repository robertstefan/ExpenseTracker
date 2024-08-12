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
                          (Id, Description, Amount, Date, CategoryId, IsRecurrent, TransactionType)
                   VALUES (@Id, @Description, @Amount, @Date, @CategoryId, @IsRecurrent, @Type)";

      await connection.ExecuteAsync(query, transaction);

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
            t.Id, 
            t.Description, 
            t.Amount, 
            t.Date, 
            t.IsRecurrent, 
            t.TransactionType AS Type,
            t.CategoryId, -- CategoryId from Transaction
            c.Id AS Id, -- CategoryId from Category
            c.Name AS Name
        FROM [Transactions] t
        JOIN Categories c ON t.CategoryId = c.Id";

      return await conn.QueryAsync<Transaction, Category, Transaction>(
          query,
          (transaction, category) =>
          {
            transaction.CategoryId = category.Id; // Map the CategoryId
            transaction.Category = category;      // Assign the Category object
            return transaction;
          },
          splitOn: "CategoryId"
      );
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = @"
        SELECT 
            t.Id, 
            t.Description, 
            t.Amount, 
            t.Date, 
            t.IsRecurrent, 
            t.TransactionType AS Type,
            t.CategoryId, -- CategoryId from Transaction
            c.Id AS Id, -- CategoryId from Category
            c.Name AS Name
        FROM [Transactions] t
        JOIN Categories c ON t.CategoryId = c.Id
        WHERE t.Id = @Id";

      var result = await conn.QueryAsync<Transaction, Category, Transaction>(
          query,
          (transaction, category) =>
          {
            transaction.Category = category; // Set the navigation property
            return transaction;
          },
          new { Id = transactionId },
          splitOn: "CategoryId" // This tells Dapper where the split between Transaction and Category occurs
      );

      return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType type)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $@"SELECT 
            t.Id, 
            t.Description, 
            t.Amount, 
            t.Date, 
            t.IsRecurrent, 
            t.TransactionType AS Type,
            t.CategoryId, -- CategoryId from Transaction
            c.Id AS Id, -- CategoryId from Category
            c.Name AS Name
        FROM [Transactions] t
        JOIN Categories c ON t.CategoryId = c.Id
        WHERE TransactionType = @TypeId";

      return await conn.QueryAsync<Transaction, Category, Transaction>(
          query,
          (transaction, category) =>
          {
            transaction.CategoryId = category.Id; // Map the CategoryId
            transaction.Category = category;      // Assign the Category object
            return transaction;
          },
          new {TypeId = (int)type},
          splitOn: "CategoryId"
      );
    }

    public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $@"UPDATE {TableName}
                              SET 
                                  Description = @Description,
                                  Amount = @Amount,
                                  Date = @Date,
                                  CategoryId = @CategoryId,
                                  IsRecurrent = @IsRecurrent,
                                  TransactionType = @Type
                              
                             WHERE Id = @Id";

      var result = await conn.ExecuteAsync(query, transaction);

      if (result == 0)
      {
        return null;
      }

      return transaction;
    }
  }
}
