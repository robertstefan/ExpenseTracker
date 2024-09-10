using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Interfaces.UserContracts;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class AuthenticationService(
    IUserSecurityRepository _userSecurityRepository,
    IUserAccountSettingsRepository _userAccountSettingsRepository,
    IUserManagementRepository _userManagementRepository,
    IActionCodeRepository _actionCodeRepository,
    IJwtTokenGenerator _jwtTokenGenerator)
{
    public async Task<AuthenticationResponse?> LoginAsync(string email, string password)
    {
        var user = await _userManagementRepository.GetUserByEmailAsync(email);

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
            await _userSecurityRepository.IncrementFailedLoginAttemptsAsync(user.Id);

            if (user.LoginTries + 1 >= 3)
            {
                await _userSecurityRepository.LockUserAsync(user.Id);
            }

            return null;
        }

        var token = _jwtTokenGenerator.GenerateToken(user);
        await _userSecurityRepository.ResetLoginAttemptsAsync(user.Id);
        return new AuthenticationResponse(user, token);
    }
    public async Task<AuthenticationResponse?> Register(User user)
    {
        var createdUser = await _userManagementRepository.AddUserAsync(user);

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

        var updatedPassword = await _userAccountSettingsRepository.ResetPasswordAsync(UserId.Value, password);

        return updatedPassword;

    }
    public async Task<bool> ChangeEmailAsync(int code, string password)
    {
        Guid? UserId = await _actionCodeRepository.UseCodeAsync(code, (int)ActionCodeType.ChangeEmail);

        if (UserId is null || !UserId.HasValue)
        {
            return false;
        }

        var updatedPassword = await _userAccountSettingsRepository.ChangeEmailAsync(UserId.Value, password);

        return updatedPassword;

    }
    public async Task<bool> UnlockUserAsync(Guid id)
    {
        return await _userSecurityRepository.UnlockUserAsync(id);
    }
}
