using System.Text.RegularExpressions;

namespace ExpenseTracker.API.DTOs;

public class UserDTO
{
  public string Username { get; set; }
  public string Email { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Password { get; set; }

  public Dictionary<string, string> Validate()
  {
    var errors = new Dictionary<string, string>();

    if (string.IsNullOrEmpty(Username)) errors.Add(nameof(Username), "Username cannot be null or empty.");
    
    if (Username.Length > 15) errors.Add(nameof(Username), "Username is too long. Maximum 15 characters allowed.");

    if (string.IsNullOrEmpty(Email))
      errors.Add(nameof(Email), "Email cannot be null or empty.");
    else if (!IsValidEmail(Email)) errors.Add(nameof(Email), "Email format is invalid.");

    if (string.IsNullOrEmpty(LastName)) errors.Add(nameof(LastName), "Last name cannot be null or empty.");
    
    if (LastName.Length > 50) errors.Add(nameof(LastName), "Last name is too long. Maximum 50 characters allowed.");

    if (string.IsNullOrEmpty(FirstName)) errors.Add(nameof(FirstName), "First name cannot be null or empty.");
    
    if (FirstName.Length > 50) errors.Add(nameof(FirstName), "First name is too long. Maximum 50 characters allowed.");

    if (string.IsNullOrEmpty(Password))
      errors.Add(nameof(Password), "Password cannot be null or empty.");
    else if (Password.Length < 8) errors.Add(nameof(Password), "Password must be at least 8 characters long.");

    return errors;
  }

  private static bool IsValidEmail(string email)
  {
    var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    return emailRegex.IsMatch(email);
  }
}