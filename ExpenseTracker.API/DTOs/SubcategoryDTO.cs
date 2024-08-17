using ExpenseTracker.API.Common.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.API.DTOs;

public class SubcategoryDTO : IEntityDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public DateTimeOffset? CreatedDateTime { get; set; }
    public DateTimeOffset? UpdatedDateTime { get; set; }

    public SubcategoryDTO()
    {

    }

    public SubcategoryDTO(Subcategory subCategory)
    {
        Id = subCategory.Id;
        Name = subCategory.Name;
        CategoryId = subCategory.CategoryId;
        CreatedDateTime = subCategory.CreatedDateTime;
        UpdatedDateTime = subCategory.UpdatedDateTime;
    }

    public SubcategoryDTO(Subcategory subCategory, Guid id)
    {
        Id = id;
        Name = subCategory.Name;
        CategoryId = subCategory.CategoryId;
        CreatedDateTime = subCategory.CreatedDateTime;
        UpdatedDateTime = subCategory.UpdatedDateTime;
    }
}