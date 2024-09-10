using ExpenseTracker.API.DTOs;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Messaging.Common;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;


namespace ExpenseTracker.API.Tasks;

public class MessageReceiverService(
    IModel _channel,
    IDistributedCache _distributedCache,
    ICurrencyExchangeProvider _exchangeProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"A message has been received {message}");

            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine($"Received message is null !");
            }
            else
            {
                ProcessMessage(message);
            }
        };

        _channel.BasicConsume(GlobalConstants.RabbitMQQueues.Currencies, true, consumer);

        await Task.CompletedTask;
    }

    private void ProcessMessage(string message)
    {
        switch (message)
        {
            case GlobalConstants.RabbitMQMessages.RefreshCurrencies:

                string? ratesSerialized = _distributedCache.GetString("Currencies");

                Currency? rates =
                    !string.IsNullOrEmpty(ratesSerialized)
                    ? JsonConvert.DeserializeObject<Currency>(ratesSerialized)
                    : null;

                if (rates != null && !rates.RatesHash.IsEmpty)
                {
                    _exchangeProvider.Set(rates);
                }
                else
                {
                    Console.WriteLine("No valid rates found in the cache.");
                }
                break;

            default:
                Console.WriteLine($"Unhandled message received: {message}");
                break;
        }
    }

}
