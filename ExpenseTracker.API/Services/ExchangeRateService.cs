using ExpenseTracker.Core.Interfaces;
using System.Xml.Serialization;
using ExpenseTracker.API.Services;
using ExpenseTracker.Core.Services;

public class ExchangeRatesService : BackgroundService
{
  private readonly IServiceProvider _serviceProvider;
  private readonly TimeSpan _scheduledTime;

  public ExchangeRatesService(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
    _scheduledTime = TimeSpan.Parse("13:00"); // Set the time you want the service to run
  }

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
      Console.WriteLine($"Next run scheduled at {nextRun}");

      await Task.Delay(delay, stoppingToken);
      if (!stoppingToken.IsCancellationRequested)
      {
        await RunUpdateAsync(stoppingToken);
      }
    }
  }

  private async Task RunUpdateAsync(CancellationToken stoppingToken)
  {
    try
    {
      Console.WriteLine("Starting exchange rate update.");

      using (var client = new HttpClient())
      {
        string xmlData = await client.GetStringAsync("https://www.bnr.ro/nbrfxrates.xml");

        XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
        DataSet exchangeRates;

        using (var reader = new StringReader(xmlData))
        {
          exchangeRates = (DataSet)serializer.Deserialize(reader);
        }

        DataSetBodyCubeRate[]? rates = exchangeRates?.Body?.Cube?.Rate;
        using (var scope = _serviceProvider.CreateScope())
        {
          var exchangeRateCache = scope.ServiceProvider.GetRequiredService<IExchangeRatesCache>();
          if (rates != null)
          {
            var exchangeRatesDictionary = new Dictionary<string, double>
            {
              ["RON"] = 1.0d
            };

            foreach (var rate in rates)
            {
              exchangeRatesDictionary[rate.currency] =
                (double)(rate.multiplierSpecified ? rate.Value * rate.multiplier : rate.Value);
            }

            await exchangeRateCache.SetExchangeRatesAsync(exchangeRatesDictionary);

            Console.WriteLine("Exchange rates updated successfully.");
          }
          else
          {
            Console.WriteLine("No rates found in the response.");
          }
        }
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      Console.WriteLine("Error updating exchange rates.");
    }
  }
}