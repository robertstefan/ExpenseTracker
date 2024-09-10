using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
    public class RaportService(IReportRepository _raportRepository)
    {
        public async Task<Raport> GetAllTransactionsHistory(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
        {
            return await _raportRepository.GenerateRaport(raportTimespanType, userId, day, month, year, weekNumber, raportPeriod);
        }

        public async Task<IEnumerable<Category>> TopCategories(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
        {
            return await _raportRepository.TopCategories(raportTimespanType, userId, day, month, year, weekNumber, raportPeriod);
        }
    }
}