using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CurrencyService.Common;
using CurrencyService.Models;

namespace CurrencyService.Provider;

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

    public DateTime SetDate { get; set; }

    public CurrencyExchangeProvider()
    {
        ExchangeRates = [];
    }

    public void Set(CurrencyDTO currencies)
    {
        ExchangeRates.Clear();

        SetDate = currencies.SetDate;

        foreach (var rate in currencies.RatesHash)
        {
            ExchangeRates[rate.Key] = rate.Value;
        }
    }
}
