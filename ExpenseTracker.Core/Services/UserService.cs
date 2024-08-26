using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class UserService(IUserRepository _userRepository)
{
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<bool> RemoveUserAsync(Guid userId, bool softDelete)
    {
        try
        {
            var res = await _userRepository.RemoveUserAsync(userId, softDelete);

            return res;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        return await _userRepository.GetUsersPaginatedAsync(pageNumber, pageSize);
    }
}
