using System.Text.RegularExpressions;
using ExpenseTracker.API.Requests.Common;

namespace ExpenseTracker.API.Requests.Categories;
public record UpdateCategoryRequest(string Name, Guid ParentCategoryId) : ValidationMiddleware
{
    protected override Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(Name))
        {            errors[nameof(Name)] = "The category name cannot be empty.";
        }

        if (Name.Length > 15)
        {
            errors[nameof(Name)] = "The category name is too long. Maximum 15 characters";
        }

        Regex regex = new("^[a-zA-Z\\s]+$");

        if (!regex.IsMatch(Name))
        {
            errors[nameof(Name)] = "The category name cannot contain special characters";
        }
        return errors;
    }
};
