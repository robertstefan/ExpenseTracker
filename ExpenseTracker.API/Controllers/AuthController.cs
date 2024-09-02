using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Microsoft.AspNetCore.Components.Route("api/auth")]
public class AuthController(AuthService _authService) : ControllerBase
{
  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<AuthServiceResponse>> LoginUserAsync([FromBody] LoginDTO request)
  {
    var authResponse = await _authService.LoginAsync(request.Email,
      request.Password);

    if (authResponse == null)
    {
      return Unauthorized();
    }

    return Ok(authResponse);
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<ActionResult<AuthServiceResponse>> Regiser([FromBody] UserDTO request)
  {
    var validationErrors = request.Validate();

    if (validationErrors.Count != 0)
    {
      return BadRequest(new { Errors = validationErrors });
    }

    try
    {
      var user = new User()
      {
        Username = request.Username,
        Email = request.Email,
        PasswordHash = request.Password,
        LastName = request.LastName,
        FirstName = request.FirstName,
      };

      var authResponse = await _authService.Register(user);

      if (authResponse is null)
      {
        Console.WriteLine("NULL");
        Console.WriteLine("User could not be added");
        return NoContent();
      }

      return Ok(authResponse);
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      Console.WriteLine("User could not be added");
      return BadRequest("User could not be created");
    }
  }
}