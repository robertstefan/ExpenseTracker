﻿using System.Data.SqlClient;

using Dapper;

using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class TransactionsRespository : ITransactionsRepository
    {
        private readonly string _connectionString;
        private static string TableName => "[Transactions]";

        public TransactionsRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Guid> CreateTransactionAsync(Transaction transaction)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = $@"INSERT INTO {TableName} 
                              (Id, Description, Amount, Date, IsRecurrent, TransactionType, CategoryId)
                          VALUES (@Id, @Description,@Amount, @Date, @IsRecurrent, @TransactionType, @CategoryId)";

            await connection.ExecuteAsync(query, transaction);

            return transaction.Id;
        }

        public async Task<bool> DeleteTransactionAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"DELETE FROM {TableName} WHERE Id = @Id;";

            var affectedRows = await conn.ExecuteAsync(query, new { Id = transactionId });

            return affectedRows == 1;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName}";

            return await conn.QueryAsync<Transaction>(query);
        }

        public async Task<Transaction> GetTransactionByIdAndCategoryIdAsync(Guid transactionId, Guid categoryId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE Id = @Id AND CategoryId = @CategoryId";

            return await conn.QuerySingleAsync<Transaction>(query, new { Id = transactionId, CategoryId = categoryId });
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE TransactionType = @TransactionType";

            return await conn.QueryAsync<Transaction>(query, new { TransactionType = transactionType });
        }

        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $@"UPDATE {TableName}
                              SET 
                                  Description = @Description,
                                  Amount = @Amount,
                                  Date = @Date,
                                  IsRecurrent = @IsRecurrent,
                                  TransactionType = @TransactionType
                              
                          WHERE Id = @Id";

            var result = await conn.ExecuteAsync(query, transaction);

            if (result == 0)
            {
                return null;
            }

            return transaction;
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid transactionId)
        {
            using var conn = new SqlConnection(_connectionString);

            var query = $"SELECT * FROM {TableName} WHERE Id = @Id";

            return await conn.QuerySingleAsync<Transaction>(query, new { Id = transactionId });
        }
    }
}
