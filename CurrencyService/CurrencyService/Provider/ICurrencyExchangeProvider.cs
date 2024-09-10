using System.Collections.Concurrent;
using CurrencyService.Models;

namespace CurrencyService.Provider;

public interface ICurrencyExchangeProvider
{
    public double this[string currency] { get; }

    public ConcurrentDictionary<string, double> ExchangeRates { get; }
    public DateTime SetDate { get; set; }

    void Set(CurrencyDTO currencies);
}
