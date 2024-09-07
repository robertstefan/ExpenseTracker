using ExpenseTracker.Core.Configuration;
using ExpenseTracker.API.Services;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Data.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

  // JWT Authentication setup
  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
  {
    In = ParameterLocation.Header,
    Description = "Please enter JWT with Bearer into the field",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});

builder.Services.AddSingleton<ICurrencyExchangeProvider, CurrencyExchangeProvider>();
builder.Services.AddHostedService<ExchangeRatesService>();

builder.Services
  .AddData(builder.Configuration)
  .AddCore()
  .AddAuth(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader()
  .AllowAnyMethod()
  .AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();