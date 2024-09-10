using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Cache.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCache(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                string connection = configuration.GetConnectionString("Redis")!;
                options.Configuration = connection;
            });
            return services;
        }
    }
}