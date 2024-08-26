using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IActionCodeRepository
{
    Task<bool> AddCodeAsync(ActionCode actionCode);
    Task<Guid?> UseCodeAsync(int code, int actionType);
}
