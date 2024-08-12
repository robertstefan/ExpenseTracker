namespace ExpenseTracker.API.Requests.Categories;
public record UpdateCategoryRequest(Guid Id, string Name, bool IsDeleted);
