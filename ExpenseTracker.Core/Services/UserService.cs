using ExpenseTracker.Core.Common.Pagination;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Interfaces.UserContracts;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class UserService(
    IUserManagementRepository _userManagementRepository,
    IUserTransactionsRepository _userTransactionsRepository)
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _userManagementRepository.GetUserByEmailAsync(email);
    }
    public async Task<User?> GetUserById(Guid userId)
    {
        return await _userManagementRepository.GetUserByIdAsync(userId);
    }
    public async Task<bool> RemoveUserAsync(Guid userId, bool softDelete)
    {
        try
        {
            var res = await _userManagementRepository.RemoveUserAsync(userId, softDelete);

            return res;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<bool> UpdateUserAsync(User user)
    {
        return await _userManagementRepository.UpdateUserAsync(user);
    }
    public async Task<PaginatedResponse<User>> GetUsersPaginatedAsync(int pageNumber, int pageSize)
    {
        return await _userManagementRepository.GetUsersPaginatedAsync(pageNumber, pageSize);
    }
    public async Task<PaginatedResponse<Transaction>> GetUserTransactionPaginatedAsync(Guid id, int pageNumber, int pageSize)
    {
        return await _userTransactionsRepository.GetUserTransactionAsync(id, pageNumber, pageSize);
    }
    public async Task<PaginatedResponse<Transaction>> GetUserTransactionsByTimeRangePaginatedAsync(Guid id, DateRange dateRange, int pageNumber, int pageSize)
    {
        return await _userTransactionsRepository.GetUserTransactionsByTimeRangePaginatedAsync(id, dateRange, pageNumber, pageSize);
    }
}
