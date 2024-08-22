using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersPaginatedAsync(int PageNumber, int PageSize);
    Task<User> UpdateUserAsync(User user);
    Task<bool> RemoveUserAsync(Guid userId);

    Task IncrementFailedLoginAttemptsAsync(Guid userId);
    Task ResetLoginAttemptsAsync(Guid userId);
    Task LockUserAsync(Guid userId);
    Task UnlockUserAsync(Guid userId);
}
