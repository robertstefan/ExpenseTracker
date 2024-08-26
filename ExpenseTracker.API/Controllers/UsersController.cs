using ExpenseTracker.API.Common.Extensions;
using ExpenseTracker.API.Common.Options;
using ExpenseTracker.API.Common.Pagination;
using ExpenseTracker.API.DTOs;
using ExpenseTracker.API.Requests.Users;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
using UserModel = ExpenseTracker.Core.Models.User;

namespace ExpenseTracker.API.Controllers;

[Route("api/users")]
public class UsersController(UserService _userService, ActionCodeService _actionCodeService, IOptions<SoftDeleteSettings> softDeleteSettings, ILogger<UsersController> _logger) : ApiController
{
    private readonly SoftDeleteSettings _softDeleteSettings = softDeleteSettings.Value;

    [HttpGet("list")]
    public async Task<ActionResult<Paged<UserDTO>>> GetUsersPaginated([FromQuery] int PageNumber = 1, [FromQuery] int PageSize = 10)
    {
        return Ok();
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

    [HttpPost("update/{id}")]
    public async Task<ActionResult<UserDTO>> UpdateUserAsync(Guid id, [FromBody] UpdateUserRequest request)
    {
        var errors = request.GetValidationErrors();

        if (errors.Count != 0)
        {
            return BadRequest(errors);
        }
        if (id.IsEmpty())
        {
            return BadRequest("Id cannot be empty or null");
        }

        var user = UserModel.Create(
            id: id,
            username: request.Username,
            email: "",
            passwordHash: "",
            lastName: request.LastName,
            firstName: request.FirstName,
            lockedOut: false,
            loginTries: 0
        );

        try
        {
            var updateSuccess = await _userService.UpdateUserAsync(user);

            if (updateSuccess)
            {
                Log.Information("The transaction was updated.");
                return Ok();
            }

            Log.Error("The transaction with the id {id} could not be updated");

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"User with the id {id} could not be updated");
            return BadRequest($"User with the id {id} could not be updated");
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<int>> IssueResetPasswordCode([FromQuery] string Email)
    {
        UserModel? user = await _userService.GetUserByEmail(Email);

        if (user is null)
        {
            return BadRequest($"No user with the email {Email} was found.");
        }

        var actionCode = ActionCode.CreateNew(
            user.Id,
            (int)ActionCodeType.ResetPassword
        );

        var result = await _actionCodeService.AddCodeAsync(actionCode);

        if (result)
        {
            return Ok(actionCode.Code);
        }
        return NoContent();
    }

    [HttpPost("change-email")]
    public async Task<ActionResult<int>> IssueChangeEmailCode([FromQuery] string Email)
    {
        UserModel? user = await _userService.GetUserByEmail(Email);

        if (user is null)
        {
            return BadRequest($"No user with the email {Email} was found.");
        }

        var actionCode = ActionCode.CreateNew(
            user.Id,
            (int)ActionCodeType.ChangeEmail
        );

        var result = await _actionCodeService.AddCodeAsync(actionCode);

        if (result)
        {
            return Ok(actionCode.Code);
        }
        return NoContent();
    }

    [HttpPost("delete/{id}")]
    public async Task<ActionResult> DeleteUser(Guid id)
    {
        if (id.IsEmpty())
        {
            return BadRequest("Id could not be null or empty.");
        }
        try
        {
            var softDelete = _softDeleteSettings.SoftDelete;
            var deleteSuccess = await _userService.RemoveUserAsync(id, softDelete);

            if (deleteSuccess)
            {
                Log.Information("The user with the id {id} was deleted", id);

                return Ok();
            }
            Log.Error("The user with the id {id} could not be deleted", id);

            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Fatal("The user with the id {id} could not be deleted", id, ex);
            return BadRequest("The user could not be deleted");
        }

    }
}
