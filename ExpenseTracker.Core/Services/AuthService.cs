using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class AuthService(UserService _userService, IJwtTokenGenerator _jwtTokenGenerator)
{
  // @TODO: password hashing
  public async Task<AuthServiceResponse?> LoginAsync(string email, string password)
  {
    var user = await _userService.GetUserByEmail(email);

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
      return null;
    }

    var token = _jwtTokenGenerator.GenerateToken(user);
    return new AuthServiceResponse(){User = user,Token = token};
  }
  
  public async Task<AuthServiceResponse?> Register(User user)
  {
    var userId = await _userService.CreateUserAsync(user);
    user = await _userService.GetUserById(userId);
    Console.WriteLine("HERE");

    var token = _jwtTokenGenerator.GenerateToken(user);

    return new AuthServiceResponse(){User = user, Token = token};
  }
}