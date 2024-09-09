using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Infrastructure.Repositories;

public class SubcategoryRepository : ISubcategoryRepository
{
  private readonly string _connectionString;

  public SubcategoryRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  private string TableName => "[Subcategories]";

  public async Task<int> AddSubcategoryAsync(Subcategory subcategory)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"INSERT INTO {TableName} (Name, CategoryId)
                          VALUES (@Name, @CategoryId);
                          SELECT CAST(SCOPE_IDENTITY() as int)";

    // Execute the query and get the newly generated Id
    var newId = await connection.QuerySingleAsync<int>(query, subcategory);

    return newId;
  }

  public async Task<Subcategory> GetSubcategoryByIdAsync(int subcategoryId)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"SELECT * FROM {TableName} WHERE Id = @Id";

    return await connection.QuerySingleAsync<Subcategory>(query, new { Id = subcategoryId });
  }

  public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"SELECT * FROM {TableName}";

    return await connection.QueryAsync<Subcategory>(query);
  }

  public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(int categoryId)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"SELECT * FROM {TableName} WHERE CategoryId = @CategoryId";

    return await connection.QueryAsync<Subcategory>(query, new { CategoryId = categoryId });
  }

  public async Task<Subcategory?> UpdateSubcategoryAsync(Subcategory subcategory)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"UPDATE {TableName}
                          SET Name = @Name, CategoryId = @CategoryId
                          WHERE Id = @Id";

    var affectedRows = await connection.ExecuteAsync(query, subcategory);

    if (affectedRows == 0) return null; // No rows were updated, return null

    return subcategory; // Return the updated subcategory
  }

  public async Task<bool> DeleteSubcategoryAsync(int subcategoryId)
  {
    using var connection = new SqlConnection(_connectionString);
    var query = $@"DELETE FROM {TableName} WHERE Id = @Id";
    var affectedRows = await connection.ExecuteAsync(query, new { Id = subcategoryId });

    return affectedRows == 1;
  }
}