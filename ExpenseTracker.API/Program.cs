using ExpenseTracker.API.Configuration;
using ExpenseTracker.API.Tasks;
using ExpenseTracker.Cache.Configuration;
using ExpenseTracker.Core.Configuration;
using ExpenseTracker.Data.Configuration;
using ExpenseTracker.Messaging.Configuration;
using Serilog;

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

try
{
    Log.Information("Expense Tracker is starting up.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();

    builder.Host.UseSerilog();

    builder.Services
    .AddData(builder.Configuration)
    .AddCore()
    .AddCache(builder.Configuration)
    .AddMessaging()
    .AddPresentation(builder.Configuration);

    builder.Services.AddHostedService<MessageReceiverService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseCors("WebPolicy");

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Expense Tracker failed to start up.");
}
finally
{
    Log.CloseAndFlush();
}
