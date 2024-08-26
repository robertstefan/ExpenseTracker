using ExpenseTracker.API.Common.Authentication;
using ExpenseTracker.API.Requests.Users;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UserModel = ExpenseTracker.Core.Models.User;

namespace ExpenseTracker.API.Controllers;

[Route("api/auth")]
public class AuthController(AuthenticationService _authService) : ApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> LoginUserAsync([FromBody] LoginUserRequest request)
    {
        var authResponse = await _authService.LoginAsync(request.Email,
        request.Password);

        if (authResponse == null)
        {
            return Unauthorized();
        }
        return new AuthenticationResult(authResponse);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResult>> CreateUserAsync([FromBody] CreateUserRequest request)
    {
        var validationErrors = request.GetValidationErrors();

        if (validationErrors.Count != 0)
        {
            return BadRequest(new { Errors = validationErrors });
        }

        try
        {
            var user = UserModel.CreateNew(
                request.Username,
                request.Email,
                request.Password,
                request.LastName,
                request.FirstName
            );

            var authResponse = await _authService.Register(user);

            if (authResponse is null)
            {
                Log.Error("User could not be added");
                return NoContent();
            }
            return new AuthenticationResult(authResponse);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "User could not be added");
            return BadRequest("User could not be created");
        }
    }

    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromQuery] int code, [FromQuery] string password)
    {
        try
        {
            var passwordUpdated = await _authService.ResetPasswordAsync(code, password);

            if (passwordUpdated)
            {
                return Ok();
            }
            return BadRequest("Password could not be updated.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Password could not be updated.");
            return BadRequest("Password could not be updated.");
        }
    }

    [AllowAnonymous]
    [HttpPost("reset-email")]
    public async Task<ActionResult> ChangeEmailAsync([FromQuery] int code, [FromQuery] string password)
    {
        try
        {
            var emailUpdated = await _authService.ChangeEmailAsync(code, password);

            if (emailUpdated)
            {
                return Ok();
            }
            return BadRequest("Email could not be updated.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Email could not be updated.");
            return BadRequest("Email could not be updated.");
        }
    }
}
