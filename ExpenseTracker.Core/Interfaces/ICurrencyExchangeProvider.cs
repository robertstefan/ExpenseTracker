using System.Collections.Concurrent;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces;

public interface ICurrencyExchangeProvider
{
    public double this[string currency] { get; }

    public ConcurrentDictionary<string, double> ExchangeRates { get; }

    public DateTime SetDate { get; }

    void Set(Currency currency);
}
