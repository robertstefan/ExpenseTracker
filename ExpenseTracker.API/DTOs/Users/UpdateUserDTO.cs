using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.API.DTOs.Users
{
    public class UpdateUserDTO
    {
        [Required]
        [MinLength(8)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }
    }
}
