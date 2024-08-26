using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Common.SharedMethods;
using static ExpenseTracker.Core.Common.Pagination.PagedMethods;

namespace ExpenseTracker.Data.Repositories;

public class CategoriesRepository(string _connectionString) : ICategoriesRepository
{
    private const string TableName = "[Categories]";
    public async Task<Guid> AddCategoryAsync(Category category)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"INSERT INTO 
                        {TableName} 
                        (Id, Name, ParentCategoryId, CreatedDateTime) 
                        VALUES 
                        (@Id, @Name, @ParentCategoryId, GETDATE())";
        await connection.ExecuteAsync(sql, category);
        return category.Id;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id, bool SoftDelete)
    {
        using var conn = new SqlConnection(_connectionString);

        int affectedRows = await ToggleDelete.Handle(id, "Categories", conn, SoftDelete);

        return affectedRows == 1;
    }

    public async Task<PaginatedResponse<Category>> GetCategoriesPaginatedAsync(int PageNumber, int PageSize)
    {
        using var connection = new SqlConnection(_connectionString);

        int Offset = OffsetMethods.GetOffsetByPageSize(PageNumber, PageSize);

        string sql = $@"
                        SELECT Count(0) [Count]
                        FROM {TableName}
                        WHERE IsDeleted = 0

                        SELECT 
                        * 
                        FROM {TableName}
                        WHERE 
                        IsDeleted = 0 
                        ORDER BY 
                        Id 
                        ASC 
                        OFFSET @Offset ROWS 
                        FETCH NEXT @PageSize 
                        ROWS ONLY";

        var multi = await connection.QueryMultipleAsync(sql, new { Offset, PageSize });

        var totalCount = multi.Read<int>().Single();

        var items = multi.Read<Category>().ToList();

        return new PaginatedResponse<Category>
        {
            TotalCount = totalCount,
            Rows = items
        };

    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @$"SELECT
                        * 
                        FROM {TableName} 
                        WHERE 
                        Id = @Id 
                        AND 
                        IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"UPDATE
                 {TableName}
                  SET 
                  Name = @Name,
                  ParentCategoryId = @ParentCategoryId,
                  UpdatedDateTime = GETDATE()
                  WHERE
                  Id = @Id
                  AND IsDeleted = 0";
        int affectedRows = await connection.ExecuteAsync(sql, category);
        return affectedRows == 1;
    }
}
