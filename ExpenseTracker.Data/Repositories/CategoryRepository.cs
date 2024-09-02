using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories;

public class CategoryRepository : ICategoryRepository
{
  private readonly string _connectionString;

  public CategoryRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  private string TableName => "[Categories]";

  public async Task<int> AddCategoryAsync(Category category)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"INSERT INTO {TableName} (Name)
                          VALUES (@Name);
                          SELECT CAST(SCOPE_IDENTITY() as int)";

    // Execute the query and get the newly generated Id
    var newId = await connection.QuerySingleAsync<int>(query, category);

    return newId;
  }

  public async Task<Category> GetCategoryByIdAsync(int categoryId)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"SELECT * FROM {TableName} WHERE Id = @Id";

    return await connection.QuerySingleAsync<Category>(query, new { Id = categoryId });
  }

  public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"SELECT * FROM {TableName}";

    return await connection.QueryAsync<Category>(query);
  }

  public async Task<Category?> UpdateCategoryAsync(Category category)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"UPDATE {TableName}
                          SET Name = @Name
                          WHERE Id = @Id";

    var affectedRows = await connection.ExecuteAsync(query, category);

    if (affectedRows == 0) return null; // No rows were updated, return null

    return category; // Return the updated category
  }

  public async Task<bool> DeleteCategoryAsync(int categoryId)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"DELETE FROM {TableName} WHERE Id = @Id";
    var affectedRows = await connection.ExecuteAsync(query, new { Id = categoryId });

    return affectedRows == 1;
  }
}