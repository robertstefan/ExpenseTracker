using ExpenseTracker.Core.Interfaces.Common;

namespace ExpenseTracker.Core.Models;

public class Subcategory : IEntity
{
    private readonly Category? _category = null;
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset? UpdatedDateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    public Guid CategoryId { get; private set; }

    public Category? Category => _category;

    private Subcategory()
    {

    }

    private Subcategory(Guid id, string name, Guid categoryId, Category? category)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        _category = category;
    }

    public static Subcategory Create(Guid id, string name, Guid categoryId, Category? category = null)
    {
        return new(
            id,
            name,
            categoryId,
            category
        );
    }

    public static Subcategory CreateNew(string name, Guid categoryId, Category? category = null)
    {
        return new(
            Guid.NewGuid(),
            name,
            categoryId,
            category
        );
    }
}
