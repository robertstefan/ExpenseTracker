using System.Text;
using CurrencyService.Common;
using RabbitMQ.Client;

namespace CurrencyService.Services;

public class MessageProducer : IMessageProducer
{
    private readonly static ConnectionFactory factory = new()
    {
        HostName = "localhost",
        UserName = "user",
        Password = "strongPass",
        VirtualHost = "/"
    };
    private readonly static IConnection connection = factory.CreateConnection();
    public void SendingMessage(string message)
    {
        using var channel = connection.CreateModel();

        channel.QueueDeclare(GlobalConstants.RabbitMQQueues.Currencies, durable: true, exclusive: false, autoDelete: true);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", GlobalConstants.RabbitMQQueues.Currencies, body: body);
    }
}
