using System.Collections.Concurrent;

namespace CurrencyService.Models;

public class CurrencyDTO
{
    public ConcurrentDictionary<string, double> RatesHash { get; set; }
    public DateTime SetDate { get; set; } = DateTime.Now;

    public CurrencyDTO(ConcurrentDictionary<string, double> ratesHash, DateTime? setDate = null)
    {
        RatesHash = ratesHash;
        SetDate = setDate ?? DateTime.Now;
    }
}
