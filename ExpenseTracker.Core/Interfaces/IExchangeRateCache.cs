namespace ExpenseTracker.Core.Services;

public interface IExchangeRatesCache
{
  Task SetExchangeRatesAsync(Dictionary<string, double> rates);
  Task<Dictionary<string, double>?> GetExchangeRatesAsync();
}