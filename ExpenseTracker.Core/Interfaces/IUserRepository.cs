using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetUsersPaginatedAsync(int PageNumber, int PageSize);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> RemoveUserAsync(Guid userId, bool softDelete);

    Task IncrementFailedLoginAttemptsAsync(Guid userId);
    Task ResetLoginAttemptsAsync(Guid userId);
    Task LockUserAsync(Guid userId);
    Task UnlockUserAsync(Guid userId);

    Task<bool> ResetPassword(Guid UserId, string Password);
    Task<bool> ChangeEmail(Guid UserId, string Email);
}
