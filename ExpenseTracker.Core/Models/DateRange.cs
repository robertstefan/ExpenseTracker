using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Core.Common.Enums;

namespace ExpenseTracker.Core.Models
{
    public class DateRange(DateTime start, DateTime end)
    {
        public DateTime Start { get; init; } = start;
        public DateTime End { get; init; } = end;

    }
}