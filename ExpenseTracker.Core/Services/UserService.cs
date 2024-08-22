using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class UserService(IUserRepository _userRepository, IJwtTokenGenerator _jwtTokenGenerator)
{
    public async Task<AuthenticationResponse?> LoginAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        if (user.LockedOut)
        {
            return null;
        }

        if (user.PasswordHash != password)
        {
            await _userRepository.IncrementFailedLoginAttemptsAsync(user.Id);

            if (user.LoginTries + 1 >= 3)
            {
                await _userRepository.LockUserAsync(user.Id);
            }

            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        await _userRepository.ResetLoginAttemptsAsync(user.Id);
        return new AuthenticationResponse(user, token);
    }
    public async Task<AuthenticationResponse?> Register(User user)
    {
        var createdUser = await _userRepository.AddUserAsync(user);

        if (createdUser is null)
        {
            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResponse(createdUser, token);
    }
    public async Task<User?> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<bool> RemoveUser(Guid userId)
    {
        try
        {
            var res = await _userRepository.RemoveUserAsync(userId);

            return res;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber, int pageSize)
    {
        return await _userRepository.GetUsersPaginatedAsync(pageNumber, pageSize);
    }
}
