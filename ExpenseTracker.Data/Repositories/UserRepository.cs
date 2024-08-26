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
                      WHERE Email = @Email
                      AND IsDeleted = 0";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = email });

        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"SELECT Id, Username, Email, FirstName, LastName, LockedOut, LoginTries, CreatedDateTime, UpdatedDateTime 
                        FROM {TableName} 
                      WHERE Id = @Id
                      AND IsDeleted = 0";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });

        return user;
    }

    public async Task<bool> RemoveUserAsync(Guid userId, bool softDelete)
    {
        using var connection = new SqlConnection(_connectionString);

        string storedProcedure = "[dbo].[RemoveUser]";
        int affected = await connection.ExecuteAsync(
            storedProcedure,
            new { userId, softDelete },
            commandType: System.Data.CommandType.StoredProcedure
        );

        return affected > 0;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                      Username = @Username,
                      LastName = @LastName,
                      FirstName = @FirstName,
                      UpdatedDateTime = GETDATE()
                      WHERE Id = @Id
                      AND IsDeleted = 0";

        var affectedRows = await connection.ExecuteAsync(sql, user);

        return affectedRows == 1;
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

    public async Task<bool> ResetPassword(Guid userId, string password)
    {

        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        PasswordHash = @Password,
                        UpdatedDateTime = GETDATE()
                        WHERE Id = @UserId";

        var affectedRows = await connection.ExecuteAsync(sql, new { UserId = userId, Password = password });

        return affectedRows == 1;
    }

    public async Task<bool> ChangeEmail(Guid UserId, string Email)
    {

        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        Email = @Email,
                        UpdatedDateTime = GETDATE()
                        WHERE Id = @UserId";

        var affectedRows = await connection.ExecuteAsync(sql, new { UserId = UserId, Email = Email });

        return affectedRows == 1;


    }
}
