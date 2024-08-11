using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface IRaportRepository
    {
        Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType);
    }
}