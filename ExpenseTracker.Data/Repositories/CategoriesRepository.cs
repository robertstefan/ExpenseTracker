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

        public async Task<Guid> CreateCategoryAsync(Category category)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = $"INSERT INTO [Categories] (Id, CategoryName) VALUES (@Id, @CategoryName)";

            await connection.ExecuteAsync(query, category);

            return category.Id;
        }

        public async Task<bool> DeleteCategoryAsync(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"DELETE FROM [Categories] WHERE Id = @Id";

            var affectedRows = await conn.ExecuteAsync(query, new { Id = categoryId });

            return affectedRows == 1;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            string query = @"
            SELECT * FROM Categories c
            LEFT JOIN Transactions t ON c.Id = t.CategoryId";

            var categoriesList = new List<Category>();

            var categories = await conn.QueryAsync<Category, Transaction, Category>(
                query,
                (category, transaction) =>
                {
                    var currentCategory = categoriesList.FirstOrDefault(c => c.Id == category.Id);

                    if (currentCategory == null)
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
                splitOn: "Id");

            return categoriesList;
        }

        public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                SELECT * FROM [Categories] c 
                LEFT JOIN [Transactions] t ON c.Id = t.CategoryId
                WHERE c.Id = @Id";

            Category category = null;

            var categories = await conn.QueryAsync<Category, Transaction, Category>(
                query,
                (categ, transaction) =>
                {
                    if (category == null)
                    {
                        category = categ;
                    }


                    if (transaction != null)
                    {
                        category.Transactions.Add(transaction);
                    }

                    return category;
                },
                new { Id = categoryId },
                splitOn: "Id"
            );
            return category;
        }

        public async Task<string> GetCategoryName(Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT CategoryName FROM [Categories] WHERE Id = @Id";

            return await conn.QueryFirstAsync<string>(query, new { Id = categoryId });
        }
    }
}
