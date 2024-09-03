namespace ExpenseTracker.Core.Interfaces
{
  public interface ICurrencyExchangeProvider
  {
    public double this[string currency] { get; }

    public Dictionary<string, double> ExchangeRates { get; }
  }
}