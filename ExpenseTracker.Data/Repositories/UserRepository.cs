using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories;

public class UserRepository(string _connectionString) : IUserRepository
{
    private const string TableName = "[Users]";
    public async Task<User?> AddUserAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"INSERT INTO {TableName} 
                        (Id, Username, Email, PasswordHash, FirstName, LastName, CreatedDateTime)
                        OUTPUT INSERTED.*
                        VALUES (@Id, @Username, @Email, @PasswordHash, @FirstName, @LastName, GETDATE())";
        var createdUser = await connection.QuerySingleOrDefaultAsync<User>(sql, user);

        return createdUser;
    }

    public async Task<IEnumerable<User>> GetUsersPaginatedAsync(int PageNumber, int PageSize)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $"SELECT Id, Username, Email, FirstName, LastName FROM {TableName}";
        var users = await connection.QueryAsync<User>(sql);

        return users;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"SELECT Id, Username, Email, PasswordHash, FirstName, LastName, LockedOut, LoginTries 
                        FROM {TableName} 
                      WHERE Email = @Email";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"SELECT Id, Username, Email, FirstName, LastName, LockedOut, LoginTries, CreatedDateTime, UpdatedDateTime 
                        FROM {TableName} 
                      WHERE Id = @Id";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });

        return user;
    }

    public async Task<bool> RemoveUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string userSql = "EXECUTE [dbo].[RemoveUser] @userId";
        int affected = await connection.ExecuteAsync(userSql, new { userId }, commandType: System.Data.CommandType.StoredProcedure);

        return affected > 0;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                      Email = @Email,
                      Username = @Username,
                      LastName = @LastName,
                      FirstName = @FirstName,
                      UpdatedDateTime = GETDATE(),
                      WHERE Id = @Id";

        await connection.ExecuteAsync(sql, user);

        return user;
    }

    public async Task IncrementFailedLoginAttemptsAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LoginTries = LoginTries + 1
                        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = userId });
    }

    public async Task LockUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LockedOut = 1
                        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = userId });
    }

    public async Task ResetLoginAttemptsAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LoginTries = 0
                        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = userId });
    }

    public async Task UnlockUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LockedOut = 0
                        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = userId });
    }
}
