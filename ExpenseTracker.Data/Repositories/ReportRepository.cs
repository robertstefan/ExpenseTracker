using System.Data.SqlClient;
using System.Globalization;

using Dapper;

using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
  public class ReportRepository(string _connectionString) : IRaportRepository
  {
    public Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType, int userId, DateTime? day, int? year, int? weekNumber, DateRange? raportPeriod)
    {
      using var connection = new SqlConnection(_connectionString);
      string sql = $@"SELECT t.*, c.Id SplitID, c.*, u.Id SplitId, u.Id
                        FROM [Transactions] t
                        INNER JOIN [Categories] c ON t.Categoryid = c.Id
                        LEFT JOIN [Categories] sC ON sC.ParentId = c.ParentId
                        INNER JOIN [Users] u ON t.UserId = u.Id
                        WHERE u.Id = @UserId 
                            AND /**date-range**/";

      DateRange? dateRange = null;

      switch (raportTimespanType)
      {
        case RaportTimespanType.Day:
          sql = sql.Replace("/**date-range**/", "Date = @Date");
          dateRange = new DateRange(DateTime.Today, DateTime.Today.AddDays(1).AddSeconds(-1));
          break;
        case RaportTimespanType.Year:
          sql = sql.Replace("/**date-range**/", "DATEPART(YEAR, Date) = @Year");
          dateRange = new DateRange(new DateTime(year.Value, 1, 1), new DateTime(year.Value, 12, 31));
          break;
        default:
        case RaportTimespanType.Month:
          sql = sql.Replace("/**date-range**/", "DATEPART(MONTH, Date) = DATEPART(MONTH, @Month) AND DATEPART(YEAR, Date) = DATEPART(YEAR, GETDATE())");
          dateRange = new DateRange(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1));
          break;
        case RaportTimespanType.Week:
          sql = sql.Replace("/**date-range**/", "DATEPART(wk, Date) = @WeekNumber");

          /**
           * @HACK - this should take some customization based on user calendar settings!
           */
          DateTime firstDayOfWeek = ISOWeek.ToDateTime(DateTime.Now.Year, weekNumber.Value, DayOfWeek.Monday);
          dateRange = new DateRange(firstDayOfWeek, firstDayOfWeek.AddDays(7));
          break;
        case RaportTimespanType.Custom:
          sql = sql.Replace("/**date-range**/", "Date >= @StartDate AND Date <= @EndDate");
          dateRange = raportPeriod;
          break;
      }

      List<Transaction> _transactions = new List<Transaction>();

      connection.QueryAsync<Transaction, Category, Category?, Transaction>(sql, (T, C, sC) =>
      {
        //if (sC != null)
        //  sC.ParentId = C.Id;

        T.Category = C;

        _transactions.Add(T);

        return T;
      }, new
      {
        Date = day.HasValue ? day.Value.Date : DateTime.Now.Date,
        Year = year.HasValue ? year.Value : DateTime.Now.Year,
        Month = DateTime.Now.Month,
        WeekNumber = weekNumber.HasValue ? weekNumber.Value : CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
        StartDate = raportPeriod?.Start,
        EndDate = raportPeriod?.End,
      });

      var income = _transactions.Where(x => ((int)x.TransactionType) < 5).Sum(x => x.Amount);
      var outcome = _transactions.Where(x => ((int)x.TransactionType) > 5).Sum(x => x.Amount);

      Raport result = Raport.Create(Guid.NewGuid(), income, outcome, dateRange, (int)raportTimespanType);

      return Task.FromResult(result);
    }
  }
}
