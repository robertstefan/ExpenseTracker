using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Common.Authentication;

public class AuthServiceResponse
{
  public User User { get; set; }
  public string Token { get; set; }
}