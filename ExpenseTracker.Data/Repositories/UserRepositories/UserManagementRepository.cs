using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces.UserContracts;
using ExpenseTracker.Core.Models;
using static ExpenseTracker.Core.Common.Pagination.PagedMethods;

namespace ExpenseTracker.Data.Repositories.UserRepositories;

public class UserManagementRepository(string _connectionString) : IUserManagementRepository
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

    public async Task<PaginatedResponse<User>> GetUsersPaginatedAsync(int PageNumber, int PageSize)
    {
        using var connection = new SqlConnection(_connectionString);

        int Offset = OffsetMethods.GetOffsetByPageSize(PageNumber, PageSize);

        string sql = @$"SELECT Count(0) [Count]
                        FROM {TableName}
                        WHERE IsDeleted = 0

                        SELECT 
                        *
                        FROM {TableName}
                        WHERE IsDeleted = 0
                        ORDER BY 
                        Id
                        ASC
                        OFFSET @Offset ROWS
                        FETCH NEXT @PageSize
                        ROWS ONLY";

        var multi = await connection.QueryMultipleAsync(sql, new { Offset, PageSize });

        var totalCount = multi.Read<int>().Single();

        var items = multi.Read<User>().ToList();

        return new PaginatedResponse<User>
        {
            TotalCount = totalCount,
            Rows = items
        };
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
}
