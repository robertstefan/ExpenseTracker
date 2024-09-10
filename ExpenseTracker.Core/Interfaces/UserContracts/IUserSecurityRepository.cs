namespace ExpenseTracker.Core.Interfaces.UserContracts;

public interface IUserSecurityRepository
{
    Task IncrementFailedLoginAttemptsAsync(Guid userId);
    Task ResetLoginAttemptsAsync(Guid userId);
    Task LockUserAsync(Guid userId);
    Task<bool> UnlockUserAsync(Guid userId);
}
