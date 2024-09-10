using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class Currency
    {
        public ConcurrentDictionary<string, double> RatesHash { get; set; }
        public DateTime SetDate { get; set; } = DateTime.Now;
    }
}