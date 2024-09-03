using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services;

public class AuthService(UserService _userService, IJwtTokenGenerator _jwtTokenGenerator)
{
  // @TODO: password hashing
  public async Task<AuthServiceResponse?> LoginAsync(string email, string password)
  {
    var user = await _userService.GetUserByEmail(email);

    if (user is null) return null;

    if (user.LockedOut) return null;

    // @TODO: actually hash this
    if (user.PasswordHash != password) return null;

    var token = _jwtTokenGenerator.GenerateToken(user);
    return new AuthServiceResponse { User = user, Token = token };
  }

  public async Task<AuthServiceResponse?> Register(User user)
  {
    var userId = await _userService.CreateUserAsync(user);
    // @TODO: hash the password
    user = await _userService.GetUserById(userId);

    var token = _jwtTokenGenerator.GenerateToken(user);

    return new AuthServiceResponse { User = user, Token = token };
  }
}