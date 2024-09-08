using ExpenseTracker.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Authentication
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginRequest ()
        {

        }

        public LoginRequest(User user)
        {
            Username = user.Username;
            Password = user.UserPassword;
        }
    }
}
