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
                SELECT c.*, s.*, t.* 
                FROM Categories c
                LEFT JOIN Subcategories s ON c.Id = s.CategoryId
                LEFT JOIN Transactions t ON s.Id = t.SubcategoryId";

            var categoriesList = new List<Category>();
            Category currentCategory = null;

            await conn.QueryAsync<Category, Subcategory, Transaction, Category>(
                query,
                (category, subcategory, transaction) =>
                {
                    if (currentCategory == null || currentCategory.Id != category.Id)
                    {
                        currentCategory = category;
                        currentCategory.Transactions = new List<Transaction>();

                        if (subcategory != null)
                        {
                            currentCategory.Subcategory = subcategory;
                            currentCategory.Subcategory.Transactions = new List<Transaction>();
                        }

                        categoriesList.Add(currentCategory);
                    }

                    if (transaction != null)
                    {
                        currentCategory.Transactions.Add(transaction);

                        if (currentCategory.Subcategory != null)
                        {
                            currentCategory.Subcategory.Transactions.Add(transaction);
                        }
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
                SELECT c.*, s.*, t.* 
                FROM Categories c 
                LEFT JOIN Subcategories s ON c.Id = s.CategoryId
                LEFT JOIN Transactions t ON s.Id = t.SubcategoryId
                WHERE c.Id = @Id";

            Category categoryResult = null;

            await conn.QueryAsync<Category, Subcategory, Transaction, Category>(
                query,
                (category, subcategory, transaction) =>
                {
                    if (categoryResult == null)
                    {
                        categoryResult = category;
                        categoryResult.Transactions = new List<Transaction>();

                        if (subcategory != null)
                        {
                            categoryResult.Subcategory = subcategory;
                            categoryResult.Subcategory.Transactions = new List<Transaction>();
                        }
                    }

                    if (transaction != null)
                    {
                        categoryResult.Transactions.Add(transaction);

                        if (categoryResult.Subcategory != null)
                        {
                            categoryResult.Subcategory.Transactions.Add(transaction);
                        }
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

    }
}
