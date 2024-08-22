using ExpenseTracker.API.Common;
using ExpenseTracker.API.Common.Options;

namespace ExpenseTracker.API.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddControllers();
        services.AddRouting(options => options.LowercaseUrls = true);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.Configure<SoftDeleteSettings>(configurationManager.GetSection(GlobalConstants.ConfigurationKeys.SoftDelete));
        services.AddCors();
        return services;
    }
}
