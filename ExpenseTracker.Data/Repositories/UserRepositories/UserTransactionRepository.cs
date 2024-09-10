using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces.UserContracts;
using ExpenseTracker.Core.Models;
using static ExpenseTracker.Core.Common.Pagination.PagedMethods;

namespace ExpenseTracker.Data.Repositories.UserRepositories;

public class UserTransactionRepository(string _connectionString) : IUserTransactionsRepository
{
    private const string TransactionsTableName = "[Transactions]";
    private const string CategoriesTableName = "[Categories]";

    public async Task<PaginatedResponse<Transaction>> GetUserTransactionAsync(Guid userId, int pageNumber, int pageSize)
    {
        using var conn = new SqlConnection(_connectionString);

        int offset = OffsetMethods.GetOffsetByPageSize(pageNumber, pageSize);

        var query = @$"
        SELECT COUNT(0) [Count]
        FROM {TransactionsTableName}
        WHERE IsDeleted = 0
          AND UserId = @userId;

        SELECT
            t.*,
            c.Id as CategorySplitId,
            c.*
        FROM 
            {TransactionsTableName} t
        INNER JOIN 
            {CategoriesTableName} c 
            ON t.CategoryId = c.Id
        WHERE 
            t.IsDeleted = 0
            AND t.UserId = @userId
        ORDER BY
            t.Date DESC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        var multi = await conn.QueryMultipleAsync(query, new { userId, offset, pageSize });

        var totalCount = multi.Read<int?>().Single();
        var rows = multi.Read<Transaction, Category, Transaction>(
            (transaction, category) =>
                Transaction.Create(
                    transaction.Id,
                    transaction.Description,
                    transaction.Amount,
                    transaction.Date,
                    transaction.CategoryId,
                    transaction.IsRecurrent,
                    transaction.TransactionType,
                    transaction.UserId,
                    category,
                    transaction.Currency,
                    transaction.ExchangeRate,
                    transaction.CreatedDateTime,
                    transaction.UpdatedDateTime),
            splitOn: "CategorySplitId"
        ).ToList();

        return new PaginatedResponse<Transaction>
        {
            TotalCount = totalCount ?? 0,
            Rows = rows
        };
    }

    public async Task<PaginatedResponse<Transaction>> GetUserTransactionsByTimeRangePaginatedAsync(
        Guid userId, DateRange dateRange, int pageNumber, int pageSize)
    {
        using var conn = new SqlConnection(_connectionString);

        int offset = OffsetMethods.GetOffsetByPageSize(pageNumber, pageSize);

        var query = @$"
        SELECT COUNT(0) [Count]
        FROM {TransactionsTableName}
        WHERE IsDeleted = 0;

        SELECT
            t.*,
            c.Id as CategorySplitId,
            c.*
        FROM 
            {TransactionsTableName} t
        INNER JOIN 
            {CategoriesTableName} c 
            ON t.CategoryId = c.Id
        WHERE 
            t.IsDeleted = 0
            AND t.UserId = @userId
            AND t.Date >= @StartDate 
            AND t.Date <= @EndDate
        ORDER BY
            t.Id ASC
        OFFSET @Offset ROWS
        FETCH NEXT @PageSize ROWS ONLY";

        var multi = await conn.QueryMultipleAsync(query, new
        {
            userId,
            offset,
            pageSize,
            StartDate = dateRange.Start,
            EndDate = dateRange.End
        });

        var totalCount = multi.Read<int>().Single();
        var rows = multi.Read<Transaction, Category, Transaction>(
            (transaction, category) =>
                Transaction.Create(
                    transaction.Id,
                    transaction.Description,
                    transaction.Amount,
                    transaction.Date,
                    transaction.CategoryId,
                    transaction.IsRecurrent,
                    transaction.TransactionType,
                    transaction.UserId,
                    category,
                    transaction.Currency,
                    transaction.ExchangeRate,
                    transaction.CreatedDateTime,
                    transaction.UpdatedDateTime),
            splitOn: "CategorySplitId"
        ).ToList();

        return new PaginatedResponse<Transaction>
        {
            TotalCount = totalCount,
            Rows = rows
        };
    }
}
