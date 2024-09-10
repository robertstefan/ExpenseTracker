using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Models;

public class Category : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid? ParentCategoryId { get; set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset? UpdatedDateTime { get; private set; }
    public bool IsDeleted { get; private set; }

    public double? CategoryIncome { get; private set; }

    public double? CategoryOutcome { get; private set; }

    private Category()
    {

    }

    private Category(Guid id, string name, Guid? parentCategoryId)
    {
        Id = id;
        Name = name;
        ParentCategoryId = parentCategoryId;

    }

    public static Category Create(Guid id, string name, Guid? parentCategoryId)
    {
        return new(
            id,
            name,
            parentCategoryId
        );
    }

    public static Category CreateNew(string name)
    {
        return new(
            Guid.NewGuid(),
            name.Trim(),
            null
            );
    }

}
