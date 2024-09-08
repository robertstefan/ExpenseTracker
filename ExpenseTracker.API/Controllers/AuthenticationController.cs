using ExpenseTracker.API.DTOs.Authentication;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly UsersService _usersService;

        public AuthenticationController (AuthenticationService authenticationService, UsersService usersService)
        {
            _authenticationService = authenticationService;
            _usersService = usersService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login (LoginRequest request)
        {
            var user = await _usersService.GetUserByUsernameAsync(request.Username);
            var token = await _authenticationService.Login(user);

            return new LoginResponse() { Token = token };
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register (RegisterRequest request)
        {
            User userToRegister = new User()
            {
                Username = request.Username,
                Email = request.Email,
                UserPassword = request.Password,
                LastName = request.LastName,
                FirstName = request.FirstName,
            };

            var userId = await _authenticationService.Register(userToRegister);

            if (userId == null)
            {
                return BadRequest();
            }
            return Ok(userId);
        }
    }
}
