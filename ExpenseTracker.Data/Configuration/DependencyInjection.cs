using Microsoft.Extensions.Configuration;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Data.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<ITransactionRepository, TransactionsRespository>(sp =>
            new TransactionsRespository(configuration.GetConnectionString("DefaultConnection")!));

        services.AddScoped<ICategoriesRepository, CategoriesRepository>(sp =>
            new CategoriesRepository(configuration.GetConnectionString("DefaultConnection")!));

        services.AddScoped<ISubcategoriesRepository, SubcategoriesRepository>(sp =>
        new SubcategoriesRepository(configuration.GetConnectionString("DefaultConnection")!));

        return services;
    }
}
