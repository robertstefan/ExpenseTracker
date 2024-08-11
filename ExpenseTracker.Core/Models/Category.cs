namespace ExpenseTracker.Core.Models;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset CreatedDateTime { get; private set; }
    public DateTimeOffset UpdatedDateTime { get; private set; }
    public bool IsDeleted { get; private set; }
    private Category()
    {

    }

    private Category(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Category Create(Guid id, string name)
    {
        return new(
            id,
            name
        );
    }

    public static Category CreateNew(string name)
    {
        return new(
            Guid.NewGuid(),
            name
            );
    }

}
