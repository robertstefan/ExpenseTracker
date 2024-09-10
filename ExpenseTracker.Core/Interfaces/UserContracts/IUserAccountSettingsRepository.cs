namespace ExpenseTracker.Core.Interfaces.UserContracts;

public interface IUserAccountSettingsRepository
{
    Task<bool> ResetPasswordAsync(Guid userId, string password);
    Task<bool> ChangeEmailAsync(Guid userId, string email);
}
