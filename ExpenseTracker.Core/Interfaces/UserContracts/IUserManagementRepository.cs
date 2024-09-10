using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces.UserContracts;

public interface IUserManagementRepository
{
    Task<User?> AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<PaginatedResponse<User>> GetUsersPaginatedAsync(int PageNumber, int PageSize);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> RemoveUserAsync(Guid userId, bool softDelete);

}
