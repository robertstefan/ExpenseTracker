using System.Text.Json;
using ExpenseTracker.Core.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace ExpenseTracker.Infrastructure.Cache;

public class ExchangeRatesCache : IExchangeRatesCache
{
  private readonly IDistributedCache _cache;
  private readonly TimeSpan _cacheExpiration = TimeSpan.FromHours(24); // Cache for 24 hours
  private readonly string _cacheKey = "ExchangeRates";

  public ExchangeRatesCache(IDistributedCache cache)
  {
    _cache = cache;
  }

  public async Task SetExchangeRatesAsync(Dictionary<string, double> rates)
  {
    var serializedRates = JsonSerializer.Serialize(rates);
    var options = new DistributedCacheEntryOptions
    {
      AbsoluteExpirationRelativeToNow = _cacheExpiration
    };
    await _cache.SetStringAsync(_cacheKey, serializedRates, options);
  }

  public async Task<Dictionary<string, double>?> GetExchangeRatesAsync()
  {
    var serializedRates = await _cache.GetStringAsync(_cacheKey);
    if (string.IsNullOrEmpty(serializedRates)) return null;

    return JsonSerializer.Deserialize<Dictionary<string, double>>(serializedRates);
  }
}