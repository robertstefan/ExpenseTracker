using CurrencyService.Caching;
using CurrencyService.Common;
using CurrencyService.Services;
using Quartz;

namespace CurrencyService.Cron;

public class UpdateCurrencyJob(IMessageProducer _messageProducer, ICacheManager _cacheManager) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            Console.WriteLine("Initializing FetchAndCache.");
            await _cacheManager.FetchAndCache(GlobalConstants.Urls.BNRUrl);
            Console.WriteLine("Operation Successful!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Operation failed {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Notifying new cache.");
            _messageProducer.SendingMessage(GlobalConstants.RabbitMQMessages.RefreshCurrencies);
        }
    }
}
