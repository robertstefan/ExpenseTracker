using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Services;

namespace ExpenseTracker.API.Services;

public class CurrencyExchangeProvider : ICurrencyExchangeProvider
{
  private readonly IExchangeRatesCache _cache;

  public double this[string currency] => ExchangeRates.ContainsKey(currency) ? ExchangeRates[currency] : 0.0;

  public Dictionary<string, double> ExchangeRates { get; private set; }

  public CurrencyExchangeProvider(IExchangeRatesCache cache)
  {
    _cache = cache;
    // Fetch exchange rates from the cache and initialize ExchangeRates
    LoadExchangeRates().GetAwaiter().GetResult();
  }

  private async Task LoadExchangeRates()
  {
    ExchangeRates = await _cache.GetExchangeRatesAsync();
  }
}