namespace CurrencyService.Services;

public interface IMessageProducer
{
    void SendingMessage(string message);
}
