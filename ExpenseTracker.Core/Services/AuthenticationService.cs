using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class AuthenticationService(IUserRepository _userRepository, IActionCodeRepository _actionCodeRepository, IJwtTokenGenerator _jwtTokenGenerator)
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
    public async Task<bool> ResetPasswordAsync(int code, string password)
    {
        Guid? UserId = await _actionCodeRepository.UseCodeAsync(code, (int)ActionCodeType.ResetPassword);

        if (UserId is null || !UserId.HasValue)
        {
            return false;
        }

        var updatedPassword = await _userRepository.ResetPassword(UserId.Value, password);

        return updatedPassword;

    }
    public async Task<bool> ChangeEmailAsync(int code, string password)
    {
        Guid? UserId = await _actionCodeRepository.UseCodeAsync(code, (int)ActionCodeType.ChangeEmail);

        if (UserId is null || !UserId.HasValue)
        {
            return false;
        }

        var updatedPassword = await _userRepository.ChangeEmail(UserId.Value, password);

        return updatedPassword;

    }
}
