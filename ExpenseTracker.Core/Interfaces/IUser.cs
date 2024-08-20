using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
  public interface IUserRepository
  {
    Task<int> CreateUserAsync(User user);

    Task<User?> GetUserById(int id);

    Task<User?> GetUserByEmail(string email);

    Task<User> UpdateUserAsync(User user);

    Task<bool> RemoveUserAsync(int userId);

    Task<IEnumerable<User>> GetAllUsersAsync();
  }
}
