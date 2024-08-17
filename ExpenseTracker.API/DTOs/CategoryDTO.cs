using ExpenseTracker.API.Common.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs;
public class CategoryDTO : IEntityDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public DateTimeOffset UpdatedDateTime { get; set; }

    public CategoryDTO()
    {

    }

    public CategoryDTO(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        CreatedDateTime = category.CreatedDateTime;
        UpdatedDateTime = category.UpdatedDateTime;
    }

    public CategoryDTO(Category category, Guid id)
    {
        Id = id;
        Name = category.Name;
        CreatedDateTime = category.CreatedDateTime;
        UpdatedDateTime = category.UpdatedDateTime;
    }
}
