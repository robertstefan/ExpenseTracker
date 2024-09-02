using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface IJwtTokenGenerator
{
  string GenerateToken(User user);
}