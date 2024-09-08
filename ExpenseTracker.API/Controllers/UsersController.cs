using ExpenseTracker.API.DTOs.Transactions;
using ExpenseTracker.API.DTOs.Users;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _usersService;
        private readonly TransactionsService _transactionsService;

        public UsersController(UsersService usersService, TransactionsService transactionsService)
        {
            _usersService = usersService;
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            return Ok( await _usersService.GetUsers());
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int userId)
        {
            var user = await _usersService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return BadRequest("Resource not found");
            }

            try
            {
                return Ok(new UserDTO(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving user");
            }
        }
        [HttpGet("search-by-email")]
        public async Task<ActionResult<UserDTO>> GetUserByEmail(string email)
        {
            var user = await _usersService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("Resource not found");
            }

            try
            {
                return Ok(new UserDTO(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving user");
            }
        }
        [HttpGet("search-by-username")]
        public async Task<ActionResult<UserDTO>> GetUserByUsername(string username)
        {
            var user = await _usersService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return BadRequest("Resource not found");
            }

            try
            {
                return Ok(new UserDTO(user));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving user");
            }
        }
        [HttpGet("{userId}/transactions")]
        public async Task<ActionResult<List<Transaction>>> GetTransactionsByUser(int userId)
        {
            try
            {
                return Ok(await _transactionsService.GetTransactionsByUserIdAsync(userId));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while retrieving transactions");
            }
        }
        [HttpDelete("{userId}")]
        public async Task<ActionResult> DeleteUser(int userId)
        {
            bool isSuccesfullyDeleted = await _usersService.DeleteUserAsync(userId);

            return isSuccesfullyDeleted ? NoContent() : BadRequest();
        }
        [HttpPut("{userId}")]
        public async Task<ActionResult> UpdateUser(int userId, UpdateUserDTO updateUserDTO)
        {
            var userToUpdate = await _usersService.GetUserByIdAsync(userId);

            if (userToUpdate == null)
            {
                return BadRequest("User not found");
            }

            var validationResponse = ValidateUserCredentials(updateUserDTO);

            if (validationResponse != null)
            {
                return validationResponse;
            }

            try
            {
                userToUpdate.Username = updateUserDTO.Username;
                userToUpdate.Email = updateUserDTO.Email;
                userToUpdate.FirstName = updateUserDTO.FirstName;
                userToUpdate.LastName = updateUserDTO.LastName;
                await _usersService.UpdateUserAsync(userToUpdate);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Error while updating the user");
            }


        }
        private ActionResult ValidateUserCredentials(UpdateUserDTO userDTO)
        {
            string emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            Regex regex = new Regex(emailRegex);

            if (userDTO == null)
            {
                return BadRequest("Payload can't be null");
            }

            if (!regex.IsMatch(userDTO.Email))
            {
                return BadRequest("Invalid email");
            }

            if (string.IsNullOrEmpty(userDTO.Email))
            {
                return BadRequest("Invalid email");
            }

            if (string.IsNullOrEmpty(userDTO.Username))
            {
                return BadRequest("Invalid Username");
            }

            if (string.IsNullOrEmpty(userDTO.FirstName))
            {
                return BadRequest("Invalid First Name");
            }

            if (string.IsNullOrEmpty(userDTO.LastName))
            {
                return BadRequest("Invalid Last Name");
            }

            return null;
        }
    }
}
