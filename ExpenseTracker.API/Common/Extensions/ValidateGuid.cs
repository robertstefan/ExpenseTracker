namespace ExpenseTracker.API.Common.Extensions;

public static class ValidateGuid
{
    public static bool IsEmpty(this Guid guid)
    {
        return guid == Guid.Empty;
    }
}
