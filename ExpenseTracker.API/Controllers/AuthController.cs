using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Microsoft.AspNetCore.Components.Route("api/auth")]
public class AuthController(AuthService _authService, ILogger<AuthController> logger) : ControllerBase
{
  [AllowAnonymous]
  [HttpPost("login")]
  public async Task<ActionResult<AuthServiceResponse>> LoginUserAsync([FromBody] LoginDTO request)
  {
    var authResponse = await _authService.LoginAsync(request.Email,
      request.Password);

    if (authResponse == null) return Unauthorized();

    return Ok(authResponse);
  }

  [AllowAnonymous]
  [HttpPost("register")]
  public async Task<ActionResult<AuthServiceResponse>> Regiser([FromBody] UserDTO request)
  {
    var validationErrors = request.Validate();

    if (validationErrors.Count != 0) return BadRequest(new { Errors = validationErrors });

    try
    {
      var user = new User
      {
        Username = request.Username,
        Email = request.Email,
        PasswordHash = request.Password,
        LastName = request.LastName,
        FirstName = request.FirstName
      };

      var authResponse = await _authService.Register(user);

      if (authResponse is null)
      {
        logger.LogError("User could not be created, auth response is null");
        return NoContent();
      }

      return Ok(authResponse);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "User could not be created");
      return BadRequest("User could not be created");
    }
  }
}