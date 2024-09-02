using System.Data;
using System.Data.SqlClient;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories;

public class UserRepository : IUserRepository
{
  private const string TableName = "[Users]";
  private readonly string _connectionString;

  public UserRepository(string connectionString)
  {
    _connectionString = connectionString;
  }

  public async Task<int> CreateUserAsync(User user)
  {
    using var connection = new SqlConnection(_connectionString);
    var sql = $@"
        INSERT INTO {TableName} 
        (Username, Email, PasswordHash, CreateDate, FirstName, LastName)
        VALUES 
        (@Username, @Email, @PasswordHash, GETDATE(), @FirstName, @LastName);
        SELECT CAST(SCOPE_IDENTITY() as int);";
    
    var id = await connection.ExecuteScalarAsync<int>(sql, user);

    return id;
  }

  public async Task<IEnumerable<User>> GetAllUsersAsync()
  {
    using var connection = new SqlConnection(_connectionString);
    var sql = $"SELECT Id, Username, Email, FirstName, LastName FROM {TableName}";
    var users = await connection.QueryAsync<User?>(sql);

    return users;
  }

  public async Task<User?> GetUserByEmail(string email)
  {
    using var connection = new SqlConnection(_connectionString);
    var sql = $@"SELECT Id, Username, Email, PasswordHash, FirstName, LastName, LockedOut, LoginTries 
                        FROM {TableName} 
                      WHERE Email = @Email";
    var user = await connection.QuerySingleAsync<User?>(sql, new { Email = email });

    return user;
  }

  public async Task<User?> GetUserById(int id)
  {
    using var connection = new SqlConnection(_connectionString);
    var sql = $@"SELECT Id, Username, Email, PasswordHash, FirstName, LastName, LockedOut, LoginTries 
                        FROM {TableName} 
                      WHERE Id = @Id";
    var user = await connection.QuerySingleAsync<User?>(sql, new { Id = id });

    return user;
  }

  public async Task<User> UpdateUserAsync(User user)
  {
    using var connection = new SqlConnection(_connectionString);

    user.UpdateDate = DateTime.Now;

    var sql = @"UPDATE [Users] SET
                      Email = @Email,
                      Username = @Username,
                      PasswordHash = @PasswordHash,
                      LastName = @LastName,
                      FirstName = @FirstName,
                      UpdateDate = @UpdateDate";

    await connection.ExecuteAsync(sql, user);

    return user;
  }

  public async Task<bool> RemoveUserAsync(int userId)
  {
    using var connection = new SqlConnection(_connectionString);

    var userSql = $"DELETE FROM {TableName} WHERE Id = @Id";
    var affected = await connection.ExecuteAsync(userSql, new { Id = userId },
      commandType: CommandType.StoredProcedure);

    return affected > 0;
  }
}