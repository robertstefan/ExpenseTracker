using System.Text.RegularExpressions;
using ExpenseTracker.API.Requests.Common;

namespace ExpenseTracker.API.Requests.Subcategories;

public record UpdateSubcategoryRequest(string Name, Guid CategoryId) : ValidationMiddleware
{
    protected override Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrEmpty(Name))
        {
            errors[nameof(Name)] = "The subcategory name cannot be empty.";
        }

        if (Name.Length > 15)
        {
            errors[nameof(Name)] = "The subcategory name is too long. Maximum 15 characters";
        }

        Regex regex = new("^[a-zA-Z\\s]+$");

        if (!regex.IsMatch(Name))
        {
            errors[nameof(Name)] = "The subcategory name cannot contain special characters";
        }

        if (CategoryId == Guid.Empty)
        {
            errors[nameof(CategoryId)] = "The category id coresponding to the subcategory cannot be empty";
        }

        return errors;
    }
};
