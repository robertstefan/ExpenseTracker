using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Core.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<TransactionService>();
        services.AddScoped<CategoryService>();
        services.AddScoped<UserService>();
        services.AddScoped<AuthenticationService>();
        services.AddScoped<ActionCodeService>();
        services.AddScoped<RaportService>();
        return services;
    }
}
