using Microsoft.Extensions.Configuration;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ExpenseTracker.Data.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        services.AddScoped<IUserRepository, UserRepository>(sp =>
        new UserRepository(configuration.GetConnectionString("DefaultConnection")!));

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
