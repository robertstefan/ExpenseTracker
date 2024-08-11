using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class RaportService(IRaportRepository _raportRepository)
    {
        public async Task<Raport> GetRaportAsync(string raportTimeSpan, string raportType = "Total")
        {
            var raportTimeSpanType = CastStringToRaportTimespanType(raportTimeSpan);

            var result = await _raportRepository.GenerateRaport(raportTimeSpanType);

            return result;
        }
        private static RaportTimespanType CastStringToRaportTimespanType(string transactionType)
        {
            if (!Enum.TryParse(transactionType.Trim(), true, out RaportTimespanType transactionTypeEnum))
            {
                transactionTypeEnum = RaportTimespanType.Today;
            }
            return transactionTypeEnum;
        }
    }
}