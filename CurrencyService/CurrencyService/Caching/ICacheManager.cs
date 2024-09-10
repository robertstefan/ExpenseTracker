using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyService.Caching
{
    public interface ICacheManager
    {
        Task FetchAndCache(string url);
    }
}