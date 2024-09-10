using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using CurrencyService.Common;
using CurrencyService.Models;
using CurrencyService.Provider;
using Microsoft.Extensions.Caching.Distributed;

namespace CurrencyService.Caching;

public class CacheManager(IDistributedCache _distributedCache, ICurrencyExchangeProvider _currencyExchangeProvider) : ICacheManager
{
    public async Task FetchAndCache(string url)
    {
        try
        {
            using HttpClient client = new();

            string? xmlData = await client.GetStringAsync(url);

            var currencyDto = Helper.ProcessXmlData(xmlData);

            _currencyExchangeProvider.Set(currencyDto);

            byte[] rates = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(currencyDto));

            await _distributedCache.SetAsync(GlobalConstants.Redis.CurrencyKey, rates);
        }
        catch (Exception ex)
        {
            if (!_currencyExchangeProvider.ExchangeRates.IsEmpty)
            {
                var currencyDto = new CurrencyDTO(_currencyExchangeProvider.ExchangeRates, _currencyExchangeProvider.SetDate);

                byte[] rates = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(currencyDto));

                await _distributedCache.SetAsync(GlobalConstants.Redis.CurrencyKey, rates);
            }
            throw;
        }
    }
}