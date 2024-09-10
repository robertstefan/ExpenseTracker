using Microsoft.Extensions.Configuration;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Data.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ExpenseTracker.Core.Interfaces.UserContracts;
using ExpenseTracker.Data.Repositories.UserRepositories;

namespace ExpenseTracker.Data.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, ConfigurationManager configuration)
    {
        var connString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddScoped<ITransactionRepository, TransactionsRespository>(sp =>
            new TransactionsRespository(connString));

        services.AddScoped<ICategoriesRepository, CategoriesRepository>(sp =>
            new CategoriesRepository(connString));

        services.AddScoped<IUserAccountSettingsRepository, UserAccountSettingsRepository>(sp =>
        new UserAccountSettingsRepository(connString));

        services.AddScoped<IUserManagementRepository, UserManagementRepository>(sp =>
        new UserManagementRepository(connString));

        services.AddScoped<IUserSecurityRepository, UserSecurityRepository>(sp =>
        new UserSecurityRepository(connString));

        services.AddScoped<IUserTransactionsRepository, UserTransactionRepository>(sp =>
        new UserTransactionRepository(connString));

        services.AddScoped<IActionCodeRepository, ActionCodeRepository>(sp =>
        new ActionCodeRepository(connString));

        services.AddScoped<IReportRepository, ReportRepository>(sp =>
        new ReportRepository(connString));

        services.AddAuth(configuration);

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();

        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        return services;
    }
}
