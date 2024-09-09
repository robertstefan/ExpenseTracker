using System.Xml.Serialization;
using ExpenseTracker.Core.Services;

namespace ExpenseTracker.API.Services;

public class ExchangeRatesService(IServiceProvider serviceProvider, ILogger<ExchangeRatesService> logger)
  : BackgroundService
{
  private readonly TimeSpan _scheduledTime = TimeSpan.Parse("13:00"); // Set the time you want the service to run

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    // Run on startup
    await RunUpdateAsync(stoppingToken);

    // Schedule to run daily
    while (!stoppingToken.IsCancellationRequested)
    {
      var now = DateTimeOffset.Now;
      var nextRun = now.Date.Add(_scheduledTime);
      if (now > nextRun)
        nextRun = nextRun.AddDays(1);

      var delay = nextRun - now;
      logger.LogInformation($"Next run scheduled at {nextRun}");

      await Task.Delay(delay, stoppingToken);
      if (!stoppingToken.IsCancellationRequested) await RunUpdateAsync(stoppingToken);
    }
  }

  private async Task RunUpdateAsync(CancellationToken stoppingToken)
  {
    try
    {
      logger.LogInformation("Starting exchange rate update.");

      using var client = new HttpClient();
      var xmlData = await client.GetStringAsync("https://www.bnr.ro/nbrfxrates.xml", stoppingToken);

      var serializer = new XmlSerializer(typeof(DataSet));
      DataSet exchangeRates;

      using (var reader = new StringReader(xmlData))
      {
        exchangeRates = (DataSet)serializer.Deserialize(reader);
      }

      var rates = exchangeRates?.Body?.Cube?.Rate;
      using (var scope = serviceProvider.CreateScope())
      {
        var exchangeRateCache = scope.ServiceProvider.GetRequiredService<IExchangeRatesCache>();
        if (rates != null)
        {
          var exchangeRatesDictionary = new Dictionary<string, double>
          {
            ["RON"] = 1.0d
          };

          foreach (var rate in rates)
            exchangeRatesDictionary[rate.currency] =
              (double)(rate.multiplierSpecified ? rate.Value * rate.multiplier : rate.Value);

          await exchangeRateCache.SetExchangeRatesAsync(exchangeRatesDictionary);

          logger.LogInformation("Exchange rates updated successfully.");
        }
        else
        {
          logger.LogCritical("No rates found in the response.");
        }
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error updating exchange rates.");
    }
  }
}