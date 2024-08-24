using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using System.Data.SqlClient;


namespace ExpenseTracker.Data.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly string _connectionString;

        public CategoriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            string query = @"
                SELECT c.*, t.* 
                FROM Categories c
                LEFT JOIN Transactions t ON c.Id = t.CategoryId";

            var categoriesList = new List<Category>();
            Category currentCategory = null;

            await conn.QueryAsync<Category, Transaction, Category>(
                query,
                (category, transaction) =>
                {
                    if (currentCategory == null || currentCategory.Id != category.Id)
                    {
                        currentCategory = category;
                        currentCategory.Transactions = new List<Transaction>();

                        categoriesList.Add(currentCategory);
                    }

                    if (transaction != null)
                    {
                        currentCategory.Transactions.Add(transaction);
                    }

                    return currentCategory;
                },
                splitOn: "Id"
            );

            return categoriesList;
        }
        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                SELECT c.*, t.* 
                FROM Categories c 
                LEFT JOIN Transactions t ON c.Id = t.CategoryId
                WHERE c.Id = @Id";

            Category categoryResult = null;

            await conn.QueryAsync<Category, Transaction, Category>(
                query,
                (category, transaction) =>
                {
                    if (categoryResult == null)
                    {
                        categoryResult = category;
                        categoryResult.Transactions = new List<Transaction>();
                    }

                    if (transaction != null)
                    {
                        categoryResult.Transactions.Add(transaction);
                    }

                    return categoryResult;
                },
                new { Id = categoryId },
                splitOn: "Id"
            );

            return categoryResult;
        }
        public async Task<Guid> CreateCategoryAsync(Category category)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = $"INSERT INTO [Categories] (Id, CategoryName, ParentCategoryId) VALUES (@Id, @CategoryName, @ParentCategoryId)";

            await connection.ExecuteAsync(query, category);

            return category.Id;
        }
        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"DELETE FROM [Categories] WHERE Id = @Id OR ParentCategoryId = @Id";

            var affectedRows = await conn.ExecuteAsync(query, new { Id = categoryId });

            return affectedRows != 0;
        }

    }
}
