using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Authentication
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(8)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[!@#$%^&*])[A-Za-z\\d!@#$%^&*]{6,}$")]

        public string Password { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}
