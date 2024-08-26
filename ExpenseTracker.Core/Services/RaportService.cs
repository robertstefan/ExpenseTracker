using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Core.Services
{
  public class RaportService(IRaportRepository _raportRepository)
  {
    public async Task<Raport> GetAllTransactionsHistory(RaportTimespanType raportTimespanType, int userId, DateTime? day, int? year, int? weekNumber, DateRange? raportPeriod)
    {
      // Tuple<List<Transaction>, totalBilled, totalIncome?>
      // Transaction: Transaction + Category + Subcategory

      return await _raportRepository.GenerateRaport(raportTimespanType, userId, day, year, weekNumber, raportPeriod);
    }
  }
}