using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class ActionCodeService(IActionCodeRepository _actionCodeRepository)
{
    public async Task<bool> AddCodeAsync(ActionCode actionCode)
    {
        return await _actionCodeRepository.AddCodeAsync(actionCode);
    }
}
