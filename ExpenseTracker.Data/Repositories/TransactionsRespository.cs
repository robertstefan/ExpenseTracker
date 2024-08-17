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
  private static string TableName => "[Transactions]";

  public async Task<Guid> AddTransactionAsync(Transaction transaction)
  {
    using var connection = new SqlConnection(_connectionString);
    try
    {
      var parameters = new DynamicParameters();
      parameters.Add("@Id", transaction.Id, DbType.Guid);
      parameters.Add("@Description", transaction.Description, DbType.String);
      parameters.Add("@Amount", transaction.Amount, DbType.Decimal);
      parameters.Add("@Date", transaction.Date, DbType.DateTime);
      parameters.Add("@CategoryId", transaction.CategoryId, DbType.Guid);
      parameters.Add("@SubcategoryId", transaction.SubcategoryId, DbType.Guid);
      parameters.Add("@IsRecurrent", transaction.IsRecurrent, DbType.Boolean);
      parameters.Add("@TransactionType", transaction.TransactionType, DbType.Int32);

      int affectedRows = await connection.ExecuteAsync("CreateTransaction", parameters);

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

  public async Task<bool> DeleteTransactionAsync(Guid transactionId, bool SoftDelete)
  {

    using var conn = new SqlConnection(_connectionString);

    int affectedRows = await ToggleDelete.Handle(transactionId, "Transactions", conn, SoftDelete);

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
                        c.Id as CategoryId,
                        c.Id AS SplitCategoryId,
                        c.Name,
                        c. CreatedDateTime,
                        c. UpdatedDateTime,
                        s.Id as SubcategoryId,
                        s.Id AS SplitSubcategoryId,
                        s.Name,
                        s.CreatedDateTime,
                        s.UpdatedDateTime,
                        s.CategoryId
                      FROM 
                        {TableName} t

                      INNER JOIN 
                        [Categories] c 
                        ON t.CategoryId = c.Id
                      INNER JOIN 
                        [Subcategories] s 
                        ON t.SubcategoryId = s.Id
                      WHERE t.IsDeleted = 0 AND c.IsDeleted = 0 AND s.IsDeleted = 0
                    ORDER BY
                    Id
                    ASC
                    OFFSET @Offset Rows
                    FETCH NEXT @PageSize
                    ROWS ONLY";

    /*Another idea for horizontal scaled tables would be to replace @Offset with @LastDate select TOP @Limit Where Date < @LastDate
    So we would get only the @Limit (ex 10) values from the @LastDate (15.08.2024) where Date is smaller than @LastDate
    This approach would improve performance because Offset has to scan X amount of rows
    This would be applied only to categories, subcategories and any other tables that would be plausible to have less amount of rows,
    only because this table is the most plausible to scale horizontally
    If we implement users, we can create a flag called TransactionCount where we would get the TotalCount for the paging, so we 
    can ignore the full select which is pricy*/

    var multi = await conn.QueryMultipleAsync(query, new { Offset, PageSize });

    var totalCount = multi.Read<int>().Single();
    var rows = multi.Read<Transaction, Category, Subcategory, Transaction>(
      (transaction, category, subcategory) =>
            {
              return Transaction.Create(
                transaction.Id,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.CategoryId,
                transaction.SubcategoryId,
                transaction.IsRecurrent,
                transaction.TransactionType,
                category,
                subcategory,
                transaction.CreatedDateTime,
                transaction.UpdatedDateTime
              );
            },
            splitOn: "SplitCategoryId,SplitSubcategoryId"
    ).ToList();

    return new PaginatedResponse<Transaction>
    {
      TotalCount = totalCount,
      Rows = rows
    };

    // IEnumerable<Transaction>? transactions = await TransactionsJoinCategories(new { Offset = offset, Limit = limit + 1 }, conn, query);

    // return transactions;
  }

  public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
  {
    using var conn = new SqlConnection(_connectionString);

    string query = $@"SELECT 
                        t.*,
                        c.Id as CategoryId,
                        c.Id AS SplitCategoryId,
                        c.Name,
                        c. CreatedDateTime,
                        c. UpdatedDateTime,
                        s.Id as SubcategoryId,
                        s.Id AS SplitSubcategoryId,
                        s.Name,
                        s.CreatedDateTime,
                        s.UpdatedDateTime,
                        s.CategoryId
                      FROM 
                        [Transactions] t
                      INNER JOIN 
                        [Categories] c 
                        ON t.CategoryId = c.Id
                      INNER JOIN 
                        [Subcategories] s 
                        ON t.SubcategoryId = s.Id
                      WHERE 
                        t.Id = @Id";

    Transaction? transaction = (await TransactionsJoinCategories(new { Id = transactionId }, conn, query)).FirstOrDefault();

    return transaction;
  }

  public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(int transactionType)
  {
    using var conn = new SqlConnection(_connectionString);

    var query = $"SELECT * FROM {TableName} WHERE TransactionType = @TransactionType";

    return await conn.QueryAsync<Transaction>(query, new { TransactionType = transactionType });
  }

  public async Task<bool> UpdateTransactionAsync(Transaction transaction)
  {
    using var conn = new SqlConnection(_connectionString);

    var parameters = new DynamicParameters();
    parameters.Add("@Id", transaction.Id, DbType.Guid);
    parameters.Add("@Description", transaction.Description, DbType.String);
    parameters.Add("@Amount", transaction.Amount, DbType.Decimal);
    parameters.Add("@Date", transaction.Date, DbType.DateTime);
    parameters.Add("@CategoryId", transaction.CategoryId, DbType.Guid);
    parameters.Add("@SubcategoryId", transaction.SubcategoryId, DbType.Guid);
    parameters.Add("@IsRecurrent", transaction.IsRecurrent, DbType.Boolean);
    parameters.Add("@TransactionType", transaction.TransactionType, DbType.Int32);

    int affectedRows = await conn.ExecuteAsync("UpdateTransaction", parameters);

    return affectedRows == 1;
  }
  private static async Task<IEnumerable<Transaction>?> TransactionsJoinCategories(object Data, SqlConnection conn, string query)
  {
    return await conn.QueryAsync<Transaction, Category, Subcategory, Transaction>(
            query,
            (transaction, category, subcategory) =>
            {
              return Transaction.Create(
                transaction.Id,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.CategoryId,
                transaction.SubcategoryId,
                transaction.IsRecurrent,
                transaction.TransactionType,
                category,
                subcategory,
                transaction.CreatedDateTime,
                transaction.UpdatedDateTime
              );
            },
            Data,
            splitOn: "SplitCategoryId,SplitSubcategoryId"
          );
  }
}
