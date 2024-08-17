using System.Data;
using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Common.SharedMethods;
using static ExpenseTracker.Core.Common.Pagination.PagedMethods;

namespace ExpenseTracker.Data.Repositories;

public class SubcategoriesRepository(string _connectionString) : ISubcategoriesRepository
{
    private static string TableName => "[Subcategories]";
    public async Task<Guid> AddSubcategoryAsync(Subcategory Subcategory)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"INSERT INTO 
                        {TableName} 
                        (Id, Name, CategoryId, CreatedDateTime) 
                        VALUES 
                        (@Id, @Name, @CategoryId, GETDATE())";
        await connection.ExecuteAsync(sql, Subcategory);
        return Subcategory.Id;
    }

    public async Task<bool> DeleteSubcategoryAsync(Guid id, bool SoftDelete)
    {
        using var conn = new SqlConnection(_connectionString);

        int affectedRows = await ToggleDelete.Handle(id, "Subcategories", conn, SoftDelete);

        return affectedRows == 1;
    }

    public async Task<PaginatedResponse<Subcategory>> GetSubcategoriesPaginatedAsync(int PageNumber, int PageSize)
    {
        using var connection = new SqlConnection(_connectionString);

        int Offset = OffsetMethods.GetOffsetByPageSize(PageNumber, PageSize);

        string sql = $@"SELECT 
                        COUNT(0) [Count]
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
        var rows = multi.Read<Subcategory>().Select(subcategory =>
        {
            return Subcategory.Create(
                subcategory.Id,
                subcategory.Name,
                subcategory.CategoryId
            );
        }).ToList();

        return new PaginatedResponse<Subcategory>
        {
            TotalCount = totalCount,
            Rows = rows
        };
    }

    public async Task<Subcategory?> GetSubcategoryByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @$"SELECT
                        * 
                        FROM {TableName} 
                        WHERE 
                        Id = @Id 
                        AND 
                        IsDeleted = 0";
        return await connection.QueryFirstOrDefaultAsync<Subcategory>(sql, new { Id = id });
    }

    public async Task<bool> UpdateSubcategoryAsync(Subcategory subcategory)
    {
        using var connection = new SqlConnection(_connectionString);
        var parameters = new DynamicParameters();

        parameters.Add("@Name", subcategory.Name, DbType.String);
        parameters.Add("@CategoryId", subcategory.CategoryId, DbType.Guid);

        int affectedRows = await connection.ExecuteAsync("UpdateSubcategory", parameters);

        return affectedRows > 0;
    }
}