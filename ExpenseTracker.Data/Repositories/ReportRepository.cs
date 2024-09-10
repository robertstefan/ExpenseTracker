using System.Data.SqlClient;
using System.Globalization;
using Dapper;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Common.SharedMethods;

namespace ExpenseTracker.Data.Repositories;

public class ReportRepository(string _connectionString) : IReportRepository
{
    public async Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = $@"SELECT t.*, c.Id SplitID, c.*, u.Id SplitId, u.Id
                        FROM [Transactions] t
                        INNER JOIN [Categories] c ON t.Categoryid = c.Id
                        INNER JOIN [Users] u ON t.UserId = u.Id
                        WHERE u.Id = @UserId 
                            AND /**date-range**/";

        DateRange? dateRange = SqlQueryHelper.ApplyDateRangeToSql(
            sql: ref sql,
            raportTimespanType: raportTimespanType,
            day: day,
            weekNumber: weekNumber,
            raportPeriod: raportPeriod);

        List<Transaction> _transactions = [];

        await connection.QueryAsync<Transaction, Category, Category?, Transaction>(sql, (T, C, sC) =>
        {
            var transaction = Transaction.Create(
                T.Id,
                T.Description,
                T.Amount,
                T.Date,
                T.CategoryId,
                T.IsRecurrent,
                T.TransactionType,
                T.UserId,
                C,
                T.Currency,
                T.ExchangeRate,
                T.CreatedDateTime,
                T.UpdatedDateTime
            );

            _transactions.Add(transaction);

            return T;
        }, new
        {
            Date = day.HasValue ? day.Value.Date : DateTime.Now.Date,
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month,
            WeekNumber = weekNumber ?? CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
            StartDate = raportPeriod?.Start,
            EndDate = raportPeriod?.End,
            UserId = userId
        });

        var income = _transactions.Where(x => ((int)x.TransactionType) < 4).Sum(x => x.Amount);
        var outcome = _transactions.Where(x => ((int)x.TransactionType) > 3).Sum(x => x.Amount);

        Raport result = Raport.Create(Guid.NewGuid(), income, outcome, dateRange, (int)raportTimespanType);

        return result;
    }

    public async Task<IEnumerable<Category>> TopCategories(RaportTimespanType raportTimespanType, Guid userId, DateTime? day, int? month, int? year, int? weekNumber, DateRange? raportPeriod)
    {
        using var connection = new SqlConnection(_connectionString);

        //https://stackoverflow.com/questions/16517298/conditional-sum-in-group-by-query-mssql
        var sql = @$"SELECT TOP 5 
                    c.Id, 
                    c.Name, 
                    SUM(CASE WHEN t.TransactionType < 4 THEN t.Amount ELSE 0 END) AS CategoryIncome, 
                    SUM(CASE WHEN t.TransactionType > 3 THEN t.Amount ELSE 0 END) AS CategoryOutcome
                    FROM Categories c
                    INNER JOIN Transactions t
                    ON c.Id = t.CategoryId
                    WHERE t.UserId = @userId
                    AND /**date-range**/
                    GROUP BY c.Id, c.Name
                    ORDER BY 
                    CategoryIncome DESC, 
                    CategoryOutcome DESC;";

        DateRange? dateRange = SqlQueryHelper.ApplyDateRangeToSql(
            sql: ref sql,
            raportTimespanType: raportTimespanType,
            day: day,
            weekNumber: weekNumber,
            raportPeriod: raportPeriod);

        var categories = await connection.QueryAsync<Category>(sql, new
        {
            Date = day.HasValue ? day.Value.Date : DateTime.Now.Date,
            Year = year ?? DateTime.Now.Year,
            Month = month ?? DateTime.Now.Month,
            WeekNumber = weekNumber ?? CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
            StartDate = raportPeriod?.Start,
            EndDate = raportPeriod?.End,
            UserId = userId
        });

        return categories;
    }
}
