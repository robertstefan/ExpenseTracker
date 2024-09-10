using ExpenseTracker.API.Requests.Common;

namespace ExpenseTracker.API.Requests.Users;

public record UpdateUserRequest(string Username, string FirstName, string LastName) : ValidationMiddleware
{
    protected override Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(Username))
        {
            errors.Add(nameof(Username), "Username cannot be null or empty.");
        }
        if (Username.Length > 15)
        {
            errors.Add(nameof(Username), "Username is too long. Maximum 15 characters allowed.");
        }
        if (string.IsNullOrEmpty(LastName))
        {
            errors.Add(nameof(LastName), "Last name cannot be null or empty.");
        }
        if (LastName.Length > 50)
        {
            errors.Add(nameof(LastName), "Last name is too long. Maximum 50 characters allowed.");
        }

        if (string.IsNullOrEmpty(FirstName))
        {
            errors.Add(nameof(FirstName), "First name cannot be null or empty.");
        }
        if (FirstName.Length > 50)
        {
            errors.Add(nameof(FirstName), "First name is too long. Maximum 50 characters allowed.");
        }
        return errors;
    }
}
