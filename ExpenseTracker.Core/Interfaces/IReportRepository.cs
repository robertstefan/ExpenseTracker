using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
    public interface IReportRepository
    {
        Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod);
        Task<IEnumerable<Category>> TopCategories(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod);
    }
}