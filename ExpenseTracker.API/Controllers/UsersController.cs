using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using ExpenseTracker.API.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace ExpenseTracker.API.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController(UserService _userService, ILogger<UsersController> _logger) : ControllerBase
{
  [HttpPost("update/{userId}")]
  public async Task<ActionResult<User>> Update(UserDTO user, int userId)
  {
    User? _user = await _userService.GetUserById(userId);

    if (user == null) return BadRequest("Wrong user identifier");

    if (_user?.Id != userId) return BadRequest("No match found");

    _user.Email = user.Email;
    _user.FirstName = user.FirstName;
    _user.LastName = user.LastName;
    _user.PasswordHash = user.Password;
    _user.Username = user.Username;

    var res = await _userService.UpdateUserAsync(_user);

    return Ok(res);
  }

  [HttpGet("all")]
  public async Task<IEnumerable<User>> GetAllUsers()
  {
    return await _userService.GetAllUsersAsync();
  }
}