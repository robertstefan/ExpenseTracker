using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Common.Authentication
{
    public record AuthenticationResponse(User User, string Token)
    {

    }
}