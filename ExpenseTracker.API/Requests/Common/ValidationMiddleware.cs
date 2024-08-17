namespace ExpenseTracker.API.Requests.Common;

public abstract record ValidationMiddleware
{
    protected abstract Dictionary<string, string> Validate();

    public Dictionary<string, string> GetValidationErrors()
    {
        return Validate();
    }
}
