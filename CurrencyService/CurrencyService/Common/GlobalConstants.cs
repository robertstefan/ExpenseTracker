namespace CurrencyService.Common;

public static class GlobalConstants
{
    public static class Urls
    {
        public static readonly string BNRUrl = "https://www.bnr.ro/nbrfxrates.xml";
    }

    public static class Redis
    {
        public static readonly string CurrencyKey = "Currencies";
    }

    public static class Cron
    {
        public static readonly int RefetchIntervalInSeconds = 5;
        public static readonly int RefetchIntervalInMinutes = 5;
        public static readonly int RefetchIntervalInHours = 5;
    }

    public static class RabbitMQQueues
    {
        public const string Currencies = "currency";
    }

    public static class RabbitMQMessages
    {
        public const string RefreshCurrencies = "refresh-currencies";
    }
}
