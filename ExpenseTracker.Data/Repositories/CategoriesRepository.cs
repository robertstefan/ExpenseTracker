using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories;

public class CategoriesRepository(string _connectionString) : ICategoriesRepository
{
    private static string TableName => "[Categories]";
    public async Task<Guid> AddCategoryAsync(Category category)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"INSERT INTO 
                        {TableName} 
                        (Id, Name, CreatedDateTime, UpdatedDateTime) 
                        VALUES 
                        (@Id, @Name, GETDATE(), GETDATE())";
        await connection.ExecuteAsync(sql, category);
        return category.Id;
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @$"UPDATE {TableName}
                        SET
                        IsDeleted = 1
                        WHERE Id = @Id";
        int affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
        return affectedRows == 1;
    }

    public async Task<IEnumerable<Category>> GetCategoriesPaginatedAsync(int offset, int limit)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"SELECT 
                        * 
                        FROM {TableName} 
                        WHERE 
                        IsDeleted = 0 
                        ORDER BY 
                        Id 
                        ASC 
                        OFFSET @Offset ROWS 
                        FETCH NEXT @Limit 
                        ROWS ONLY";
        return await connection.QueryAsync<Category>(sql, new { Offset = offset, Limit = limit });

    }

    public async Task<Category> GetCategoryByIdAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @$"SELECT
                        * 
                        FROM {TableName} 
                        WHERE 
                        Id = @Id 
                        AND 
                        IsDeleted = 0";
        return await connection.QueryFirstAsync<Category>(sql, new { Id = id });
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"UPDATE
                 {TableName}
                  SET 
                  Name = @Name,
                  IsDeleted = @IsDeleted
                  UpdatedDateTime = GETDATE()
                  WHERE
                  Id = @Id";
        int affectedRows = await connection.ExecuteAsync(sql, category);
        return affectedRows == 1;
    }
}
