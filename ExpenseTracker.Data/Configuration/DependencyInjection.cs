using System.Text;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Services;
using ExpenseTracker.Data.Authentication;
using ExpenseTracker.Data.Cache;
using ExpenseTracker.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTracker.Data.Configuration;

public static class DependencyInjection
{
  public static IServiceCollection AddData(this IServiceCollection services, ConfigurationManager configuration)
  {
    services.AddScoped<ITransactionRepository, TransactionsRepository>(sp =>
      new TransactionsRepository(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ICategoryRepository, CategoryRepository>(sp =>
      new CategoryRepository(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<ISubcategoryRepository, SubcategoryRepository>(sp =>
      new SubcategoryRepository(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IUserRepository, UserRepository>(sp =>
      new UserRepository(configuration.GetConnectionString("DefaultConnection")!));
    services.AddStackExchangeRedisCache(options =>
    {
      options.Configuration = "localhost:6379";  // Redis connection string
      options.InstanceName = "RedisExchangeRates_";  // Optional: a prefix for keys
    });


    services.AddScoped<IExchangeRatesCache, ExchangeRatesCache>();
    return services;
  }

  public static IServiceCollection AddAuth(this IServiceCollection services, ConfigurationManager configuration)
  {
    var JwtKey = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

    services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

    services.AddSingleton<JwtSettings>(sp => new JwtSettings
    {
      Audience = configuration["Jwt:Issuer"],
      Issuer = configuration["Jwt:Audience"],
      Secret = configuration["Jwt:Key"],
      ExpiryMinutes = 60
    });

    services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = configuration["Jwt:Issuer"],
          ValidAudience = configuration["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(JwtKey)
        };
      });
    return services;
  }
}