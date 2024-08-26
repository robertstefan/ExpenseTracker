using System.Data;
using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Common.SharedMethods;
using static ExpenseTracker.Core.Common.Pagination.PagedMethods;

namespace ExpenseTracker.Data.Repositories;

public class TransactionsRespository(string _connectionString) : ITransactionRepository
{
  private const string TableName = "[Transactions]";
  private static string CategoriesTableName => "[Categories]";

  public async Task<Guid> AddTransactionAsync(Transaction transaction)
  {
    using var connection = new SqlConnection(_connectionString);

    string sql = @$"INSERT INTO {TableName}
                      (Id, Description, Amount, Date, CategoryId, IsRecurrent, TransactionType, CreatedDateTime, IsDeleted, UserId)
                      VALUES (@Id, @Description, @Amount, @Date, @CategoryId, @IsRecurrent, @TransactionType, GETDATE(), @IsDeleted, @UserId)";

    int affectedRows = await connection.ExecuteAsync(sql, transaction);

    if (affectedRows > 0)
    {
      return transaction.Id;
    }

    return Guid.Empty;
  }

  public async Task<bool> DeleteTransactionAsync(Guid transactionId, bool SoftDelete)
  {

    using var conn = new SqlConnection(_connectionString);

    int affectedRows = await ToggleDelete.Handle(transactionId, TableName, conn, SoftDelete);

    return affectedRows == 1;
  }

  public async Task<PaginatedResponse<Transaction>?> GetTransactionsPaginatedAsync(int PageNumber, int PageSize)
  {

    using var conn = new SqlConnection(_connectionString);

    int Offset = OffsetMethods.GetOffsetByPageSize(PageNumber, PageSize);

    var query = @$"     SELECT 
                        COUNT(0) [Count]
                        FROM {TableName}
                        WHERE IsDeleted = 0

                        SELECT
                        t.*,
                        c.Id as CategorySplitId,
                        c.*
                      FROM 
                        {TableName} t
                      INNER JOIN 
                        {CategoriesTableName} c 
                        ON t.CategoryId = c.Id
                      WHERE t.IsDeleted = 0
                    ORDER BY
                    t.Id
                    ASC
                    OFFSET @Offset Rows
                    FETCH NEXT @PageSize
                    ROWS ONLY";

    var multi = await conn.QueryMultipleAsync(query, new { Offset, PageSize });

    var totalCount = multi.Read<int>().Single();
    var rows = multi.Read<Transaction, Category, Transaction>(
      (transaction, category) =>
            {
              return Transaction.Create(
                transaction.Id,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.CategoryId,
                transaction.IsRecurrent,
                transaction.TransactionType,
                transaction.UserId,
                category,
                transaction.CreatedDateTime,
                transaction.UpdatedDateTime
              );
            },
            splitOn: "CategorySplitId"
    ).ToList();

    return new PaginatedResponse<Transaction>
    {
      TotalCount = totalCount,
      Rows = rows
    };
  }

  public async Task<Transaction?> GetTransactionByIdAsync(Guid transactionId)
  {
    using var conn = new SqlConnection(_connectionString);

    string query = $@" SELECT
                        t.*,
                        c.Id as CategorySplitId,
                        c.*
                      FROM 
                        {TableName} t
                      INNER JOIN 
                        {CategoriesTableName} c 
                        ON t.CategoryId = c.Id
                      WHERE t.IsDeleted = 0 AND t.Id = @Id";

    var multi = await conn.QueryMultipleAsync(query, new { Id = transactionId });

    var transaction = multi.Read<Transaction, Category, Transaction>(
      (transaction, category) =>
      {
        return Transaction.Create(
                transaction.Id,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.CategoryId,
                transaction.IsRecurrent,
                transaction.TransactionType,
                transaction.UserId,
                category,
                transaction.CreatedDateTime,
                transaction.UpdatedDateTime
        );
      },
      splitOn: "CategorySplitId"
    ).FirstOrDefault();

    return transaction;
  }

  public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
  {
    using var conn = new SqlConnection(_connectionString);

    var query = @$"SELECT * FROM {TableName} 
                  WHERE TransactionType = @TransactionType
                  AND IsDeleted = 0";

    return await conn.QueryAsync<Transaction>(query, new { TransactionType = transactionType });
  }

  public async Task<bool> UpdateTransactionAsync(Transaction transaction)
  {
    using var conn = new SqlConnection(_connectionString);

    string sql = @$"UPDATE {TableName} SET
                    Description = @Description,
                    Amount = @Amount,
                    Date = @Date,
                    CategoryId = @CategoryId,
                    TransactionType = @TransactionType,
                    IsRecurrent = @IsRecurrent,
                    IsDeleted = @IsDeleted,
                    UpdatedDateTime = GETDATE()
                    WHERE Id = @Id
                    AND IsDeleted = 0";

    int affectedRows = await conn.ExecuteAsync(sql, transaction);

    return affectedRows == 1;
  }
}
