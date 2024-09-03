
using ExpenseTracker.Core.Interfaces;

using System.Xml.Serialization;

namespace ExpenseTracker.API.Services
{
  public class ExchangeRatesService : BackgroundService
  {
    private ICurrencyExchangeProvider _exchangeProvider;

    public ExchangeRatesService(ICurrencyExchangeProvider exchangeProvider)
    {
      _exchangeProvider = exchangeProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      // @TODO - read from appSettings
      var scheduledHour = "13:00";
      TimeSpan? _scheduledTime;

      if (TimeSpan.TryParse(scheduledHour, out var runTime))
      {
        _scheduledTime = runTime;
      }
      else
      {
        _scheduledTime = TimeSpan.FromHours(1);
      }

      try
      {
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

          if (rates != null)
          {
            foreach (var rate in rates)
            {
              _exchangeProvider.ExchangeRates.Add(rate.currency, (double)(rate.multiplierSpecified ? rate.Value * rate.multiplier : rate.Value));
            }
          }
        }
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(ex.Message.ToString());
      }
    }
  }
}

