using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces.UserContracts;

namespace ExpenseTracker.Data.Repositories.UserRepositories;

public class UserAccountSettingsRepository(string _connectionString) : IUserAccountSettingsRepository
{
    private const string TableName = "[Users]";

    public async Task<bool> ResetPasswordAsync(Guid UserId, string Password)
    {
        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        PasswordHash = @Password,
                        UpdatedDateTime = GETDATE()
                        WHERE Id = @UserId";

        var affectedRows = await connection.ExecuteAsync(sql, new { UserId, Password });

        return affectedRows == 1;
    }
    public async Task<bool> ChangeEmailAsync(Guid UserId, string Email)
    {

        using var connection = new SqlConnection(_connectionString);

        string sql = $@"UPDATE {TableName} SET
                        Email = @Email,
                        UpdatedDateTime = GETDATE()
                        WHERE Id = @UserId";

        var affectedRows = await connection.ExecuteAsync(sql, new { UserId, Email });

        return affectedRows == 1;
    }
}
