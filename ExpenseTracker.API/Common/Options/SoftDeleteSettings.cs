namespace ExpenseTracker.API.Common.Options;

public class SoftDeleteSettings
{
    public const string SoftDeleteKey = GlobalConstants.ConfigurationKeys.SoftDelete;

    public bool SoftDelete { get; set; }

    public SoftDeleteSettings(IConfiguration configuration)
    {
        configuration.GetSection(SoftDeleteKey).Bind(this);
    }

    public SoftDeleteSettings()
    {
    }
}

