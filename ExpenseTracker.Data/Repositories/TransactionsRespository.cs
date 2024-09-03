using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Data.Repositories
{
  public class TransactionsRespository(string _connectionString) : ITransactionRepository
  {
    private static string TableName => "[Transactions]";

    public async Task<Guid> AddTransactionAsync(Transaction transaction)
    {
      using var connection = new SqlConnection(_connectionString);
      try
      {
        var createTransactionQuery = $@"
            IF EXISTS (
                SELECT 1 
                FROM [Categories] 
                WHERE IsDeleted = 0 
                AND Id = @CategoryId
            )
            BEGIN
                INSERT INTO {TableName}
                (Id, Description, Amount, Date, CategoryId, IsRecurrent, TransactionType, CreatedDateTime, Currency, ExchangeRate)
                VALUES 
                (@Id, @Description, @Amount, @Date, @CategoryId, @IsRecurrent, @TransactionType, GETDATE(), @Currency, @ExchangeRate)
            END";

        int affectedRows = await connection.ExecuteAsync(createTransactionQuery, transaction);

        if (affectedRows > 0)
        {
          return transaction.Id;
        }

        return Guid.Empty;

      }
      catch (Exception ex)
      {
        return Guid.Empty;
      }
    }

    public async Task<bool> DeleteTransactionAsync(Guid transactionId)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $"DELETE FROM {TableName} WHERE Id = @Id;";

      var affectedRows = await conn.ExecuteAsync(query, new { Id = transactionId });

      return affectedRows == 1;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsPaginatedAsync(int offset, int limit)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = @$"SELECT 
                      t.*,
                      c.Id SplitId,
                      c.Id AS CategoryId,
                      c.Name,
                      c.IsDeleted,
                      c.CreatedDateTime
                    FROM 
                      {TableName} t
                    INNER JOIN
                      Categories c ON t.CategoryId = c.Id
                    ORDER BY
                    Id
                    ASC
                    OFFSET @Offset Rows
                    FETCH NEXT @Limit
                    ROWS ONLY";

      List<Transaction> transactions = new List<Transaction>();

      await conn.QueryAsync<Transaction, Category, Transaction>(query, (T, C) =>
      {
        if (C != null)
        {
          T.Category = C;
        }

        transactions.Add(T);

        return T;
      }, new { Offset = offset, Limit = limit }, splitOn: "SplitID");

      return transactions;
    }

    public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = @$"SELECT 
                      t.Id as TransactionId,
                      t.Description,
                      t.Amount,
                      t.Date,
                      t.IsRecurrent,
                      t.TransactionType,
                      t.CreatedDateTime,
                      t.UpdatedDateTime,
                      t.Currency,
                      t.ExchangeRate,
                      c.Name as CategoryName
                    FROM 
                      {TableName} t
                    INNER JOIN
                      Categories c ON t.CategoryId = c.Id
                    WHERE
                      t.Id = @Id";

      return await conn.QuerySingleAsync<Transaction>(query, new { Id = transactionId });
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
    {
      using var conn = new SqlConnection(_connectionString);

      var query = $"SELECT * FROM {TableName} WHERE TransactionType = @TransactionType";

      return await conn.QueryAsync<Transaction>(query, new { TransactionType = transactionType });
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
                   TransactionType = @TransactionType,
                   UpdatedDateTime = GETDATE(),
                   Currency = @Currency,
                   ExchangeRate = @ExchangeRate
               WHERE Id = @Id";

      var result = await conn.ExecuteAsync(query, transaction);

      if (result == 0)
      {
        return null;
      }

      return transaction;
    }

    public async Task<IEnumerable<Transaction>> GetTransactionByCategoryIdAsync(Guid categoryId)
    {
      using (var conn = new SqlConnection(_connectionString))
      {
        var query = "SELECT T.*, C.Id AS SplitId, C.* FROM Transactions T INNER JOIN Categories C ON T.CategoryId = C.Id WHERE C.Id = @CategoryId";

        List<Transaction> transactions = new List<Transaction>();
        conn.Query<Transaction, Category, Transaction>(query, (T, C) =>
        {
          if (C != null)
          {
            T.Category = C;
          }

          transactions.Add(T);

          return T;
        }, new { CategoryId = categoryId }, splitOn: "SplitId");

        return transactions;
      }
    }

    public async Task<IEnumerable<Transaction>> GetTransactionsByUserId(Guid userId)
    {
      using var connection = new SqlConnection(_connectionString);
      string sql = $@"SELECT *
                        FROM {TableName} 
                      WHERE UserId = @UserId";
      var transactions = await connection.QueryAsync<Transaction?>(sql, new { UserId = userId });

      return transactions;
    }
  }
}
