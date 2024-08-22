using ExpenseTracker.API.Common.Authentication;
using ExpenseTracker.API.Common.Extensions;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Users;
using ExpenseTracker.Core.Common.Authentication;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using UserModel = ExpenseTracker.Core.Models.User;

namespace ExpenseTracker.API.Controllers;

[Route("api/users")]
public class UsersController(UserService _userService, ILogger<UsersController> _logger) : ApiController
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResult>> LoginUserAsync([FromBody] LoginUserRequest request)
    {
        var authResponse = await _userService.LoginAsync(request.Email,
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

            var authResponse = await _userService.Register(user);

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

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUserByIdAsync(Guid id)
    {
        if (id.IsEmpty())
        {
            return BadRequest("The id could not be null");
        }
        try
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
            {
                Log.Error("Could not retrieve the user with the id {id}", id);
                return NotFound("No users with the associated id were found.");
            }

            return Ok(new UserDTO(user));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Could not retrieve the user with the id {id}", id);

            return BadRequest("The user could not be retrieved");

        }
    }
}
