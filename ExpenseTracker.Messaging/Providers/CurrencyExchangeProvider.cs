using System.Collections.Concurrent;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Messaging.Providers;

public class CurrencyExchangeProvider : ICurrencyExchangeProvider
{
    public double this[string currency]
    {
        get
        {
            if (ExchangeRates.TryGetValue(currency, out var rate))
            {
                return rate;
            }
            return default;
        }
    }
    public ConcurrentDictionary<string, double> ExchangeRates { get; private set; }

    public DateTime SetDate { get; private set; }

    public CurrencyExchangeProvider()
    {
        ExchangeRates = [];
    }

    public void Set(Currency currency)
    {
        ExchangeRates.Clear();
        SetDate = currency.SetDate;
        foreach (var rate in currency.RatesHash)
        {
            ExchangeRates[rate.Key] = rate.Value;
        }
    }
}
