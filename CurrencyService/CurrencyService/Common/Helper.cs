using System.Collections.Concurrent;
using System.Xml.Serialization;
using CurrencyService.Models;

namespace CurrencyService.Common;

public static class Helper
{
    public static CurrencyDTO ProcessXmlData(string xmlData)
    {
        XmlSerializer serializer = new(typeof(DataSet));
        DataSet exchangeRates;

        using (var reader = new StringReader(xmlData))
        {
            exchangeRates = (DataSet)serializer.Deserialize(reader);
        }

        DataSetBodyCubeRate[]? rates = exchangeRates?.Body?.Cube?.Rate;

        ConcurrentDictionary<string, double> ratesHash = [];

        if (rates != null)
        {
            foreach (var rate in rates)
            {
                ratesHash.TryAdd(rate.currency, (double)(rate.multiplierSpecified ? rate.Value * rate.multiplier : rate.Value));
            }
        }

        return new CurrencyDTO(ratesHash);
    }
}
