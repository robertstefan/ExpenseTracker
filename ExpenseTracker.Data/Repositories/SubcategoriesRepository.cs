using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using System.Data.SqlClient;

namespace ExpenseTracker.Data.Repositories
{
    public class SubcategoriesRepository : ISubcategoriesRepository
    {
        private readonly string _connectionString;

        public SubcategoriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                SELECT s.*, t.* 
                FROM Subcategories s
                LEFT JOIN Transactions t ON s.Id = t.SubcategoryId";

            var subcategoriesList = new List<Subcategory>();
            Subcategory currentSubcategory = null;

            await conn.QueryAsync<Subcategory, Transaction, Subcategory>(
                query,
                (subcategory, transaction) =>
                {
                    if (currentSubcategory == null || currentSubcategory.Id != subcategory.Id)
                    {
                        currentSubcategory = subcategory;
                        currentSubcategory.Transactions = new List<Transaction>();
                        subcategoriesList.Add(currentSubcategory);
                    }

                    if (transaction != null)
                    {
                        currentSubcategory.Transactions.Add(transaction);
                    }

                    return currentSubcategory;
                },
                splitOn: "Id"
            );

            return subcategoriesList;
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid subcategoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = @"
                SELECT s.*, t.* 
                FROM Subcategories s
                LEFT JOIN Transactions t ON s.Id = t.SubcategoryId
                WHERE s.Id = @Id";

            Subcategory subcategoryResult = null;

            await conn.QueryAsync<Subcategory, Transaction, Subcategory>(
                query,
                (subcategory, transaction) =>
                {

                    if (subcategoryResult == null)
                    {
                        subcategoryResult = subcategory;
                        subcategoryResult.Transactions = new List<Transaction>();
                    }

                    if (transaction != null)
                    {
                        subcategoryResult.Transactions.Add(transaction);
                    }

                    return subcategoryResult;
                },
                new { Id = subcategoryId },
                splitOn: "Id"
            );

            return subcategoryResult;
        }

        public async Task<Guid> CreateSubcategoryAsync(Subcategory subcategory)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = $"INSERT INTO [Subcategories] (Id, SubcategoryName, CategoryId) VALUES (@Id, @SubcategoryName, @CategoryId)";

            await connection.ExecuteAsync(query, subcategory);

            return subcategory.Id;
        }

        public async Task<bool> DeleteSubcategoryAsync(Guid subcategoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = "DELETE FROM [Subcategories] WHERE Id = @Id";

            var affectedRows = await conn.ExecuteAsync(query, new { Id = subcategoryId });

            return affectedRows == 1;
        }
    }
}
