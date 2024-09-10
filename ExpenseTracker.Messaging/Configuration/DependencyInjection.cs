using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Messaging.Common;
using ExpenseTracker.Messaging.Providers;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace ExpenseTracker.Messaging.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<IConnection>(sp =>
       {
           var factory = new ConnectionFactory()
           {
               HostName = "localhost",
               UserName = "user",
               Password = "strongPass",
               VirtualHost = "/"
           };

           return factory.CreateConnection();
       });

        services.AddSingleton<IModel>(sp =>
        {
            var connection = sp.GetRequiredService<IConnection>();
            var channel = connection.CreateModel();
            channel.QueueDeclare(
                GlobalConstants.RabbitMQQueues.Currencies,
                durable: true,
                exclusive: false,
                autoDelete: true);
            return channel;
        });

        services.AddSingleton<ICurrencyExchangeProvider, CurrencyExchangeProvider>();

        return services;
    }
}
