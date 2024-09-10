using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces.UserContracts;

namespace ExpenseTracker.Data.Repositories.UserRepositories;

public class UserSecurityRepository(string _connectionString) : IUserSecurityRepository
{
    private const string TableName = "[Users]";

    public async Task IncrementFailedLoginAttemptsAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LoginTries = LoginTries + 1
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
    public async Task LockUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LockedOut = 1
                        WHERE Id = @Id";

        await connection.ExecuteAsync(sql, new { Id = userId });
    }
    public async Task<bool> UnlockUserAsync(Guid userId)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        LockedOut = 0
                        WHERE Id = @Id";

        int affectedRows = await connection.ExecuteAsync(sql, new { Id = userId });

        return affectedRows == 1;
    }
}
