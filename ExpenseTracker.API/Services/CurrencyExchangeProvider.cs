using ExpenseTracker.Core.Interfaces;

namespace ExpenseTracker.API.Services;

public class CurrencyExchangeProvider : ICurrencyExchangeProvider
{
  public double this[string currency] => ExchangeRates[currency];

  public Dictionary<string, double> ExchangeRates { get; private set; }

  public CurrencyExchangeProvider()
  {
    ExchangeRates = new Dictionary<string, double>();
  }
}