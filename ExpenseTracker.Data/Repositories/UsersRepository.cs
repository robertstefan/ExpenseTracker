using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using System.Data.SqlClient;

namespace ExpenseTracker.Data.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository (string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<User>> GetUsers ()
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"SELECT u.*, t.*
                             FROM Users u
                             LEFT JOIN Transactions t ON u.Id = t.UserId";

            var usersList = new List<User>();
            User currentUser = null;

            await connection.QueryAsync<User, Transaction, User>(query, (user, transaction) =>
            {
                if (currentUser == null)
                {
                    currentUser = user;
                    currentUser.Transactions = new List<Transaction>();

                    usersList.Add(currentUser);
                }

                if (transaction != null)
                {
                    currentUser.Transactions.Add(transaction);
                }

                return currentUser;
            }, splitOn: "Id");
            return usersList;
        }

        public async Task<bool> DeleteUserAsync (int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = "EXEC [dbo].[RemoveUser] @userId";
            int affected = await connection.ExecuteAsync(query, new { userId = userId });

            return affected > 0;
        }

        public async Task<User> GetUserByEmailAsync (string email)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"SELECT u.*, t.*
                             FROM Users u
                             LEFT JOIN Transactions t ON u.Id = t.UserId
                             WHERE u.Email = @Email";

            User userResult = null;

            await connection.QueryAsync<User, Transaction, User>(
                query, (user, transaction) =>
                {
                    if (userResult == null)
                    {
                        userResult = user;
                        userResult.Transactions = new List<Transaction>();
                    }

                    if (transaction != null)
                    {
                        userResult.Transactions.Add(transaction);
                    }
                    return userResult;
                }, new { Email = email }, splitOn: "Id");
            return userResult;
        }

        public async Task<User> GetUserByIdAsync (int userId)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"SELECT u.*, t.*
                             FROM Users u
                             LEFT JOIN Transactions t ON u.Id = t.UserId
                             WHERE u.Id = @Id";

            User userResult = null;

            await connection.QueryAsync<User, Transaction, User>(
                query, (user, transaction) =>
                {
                    if (userResult == null)
                    {
                        userResult = user;
                        userResult.Transactions = new List<Transaction>();
                    }

                    if (transaction != null)
                    {
                        userResult.Transactions.Add(transaction);
                    }
                    return userResult;
                }, new { Id = userId }, splitOn: "Id");
            return userResult;
        }

        public async Task<User> GetUserByUsernameAsync (string username)
        {
            using var connection = new SqlConnection(_connectionString);

            string query = @"SELECT u.*, t.*
                             FROM Users u
                             LEFT JOIN Transactions t ON u.Id = t.UserId
                             WHERE u.Username = @Username";

            User userResult = null;

            await connection.QueryAsync<User, Transaction, User>(
                query, (user, transaction) =>
                {
                    if (userResult == null)
                    {
                        userResult = user;
                        userResult.Transactions = new List<Transaction>();
                    }

                    if (transaction != null)
                    {
                        userResult.Transactions.Add(transaction);
                    }
                    return userResult;
                }, new { Username = username }, splitOn: "Id");
            return userResult;
        }

        public async Task<User> UpdateUserAsync (User user)
        {
            using var connection = new SqlConnection(_connectionString);

            user.UpdateDate = DateTime.Now;

            string query = @"UPDATE Users SET
                             Email = @Email,
                             Username = @Username,
                             PasswordHash = @PasswordHash,
                             LastName = @LastName,
                             FirstName = @FirstName,
                             UpdateDate = @UpdateDate";

            await connection.ExecuteAsync(query, user);

            return user;
        }

        public async Task<int> CreateUserAsync (User user)
        {
            using var connection = new SqlConnection(_connectionString);

            var query = @"INSERT INTO Users (Username, Email, UserPassword, CreateDate, FirstName, LastName)
                        VALUES (@Username, @Email, @UserPassword, GETDATE(), @FirstName, @LastName);
                        SELECT @@IDENTITY AS Id";

            var id = await connection.ExecuteScalarAsync<int>(query, user);

            return id;
        }
    }
}
