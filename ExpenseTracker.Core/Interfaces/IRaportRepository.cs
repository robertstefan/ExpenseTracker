using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Interfaces
{
  public interface IRaportRepository
  {
    Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType, int userId, DateTime? day, int? year, int? weekNumber, DateRange? raportPeriod);
  }
}