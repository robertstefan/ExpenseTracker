namespace ExpenseTracker.Messaging.Common;

public static class GlobalConstants
{
    public static class RabbitMQQueues
    {
        public const string Currencies = "currency";
    }

    public static class RabbitMQMessages
    {
        public const string RefreshCurrencies = "refresh-currencies";
    }
}
