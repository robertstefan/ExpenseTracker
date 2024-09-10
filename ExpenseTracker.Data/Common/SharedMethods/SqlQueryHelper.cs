using System.Globalization;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Common.SharedMethods;

public static class SqlQueryHelper
{
    public static DateRange? ApplyDateRangeToSql(
        ref string sql,
        RaportTimespanType raportTimespanType,
        DateTime? day,
        int? weekNumber,
        DateRange? raportPeriod)
    {
        DateRange dateRange = null!;

        switch (raportTimespanType)
        {
            case RaportTimespanType.Today:
                sql = sql.Replace("/**date-range**/", "CAST(Date AS DATE) = CAST(GETDATE() AS DATE)");
                dateRange = new DateRange(
                    start: DateTime.Today,                          //Today from 00:00:00
                    end: DateTime.Today.AddDays(1).AddSeconds(-1)   //Today at 25:59:59

                );
                break;
            case RaportTimespanType.Day:
                sql = sql.Replace("/**date-range**/", "Date = @Date");
                dateRange = new DateRange(
                    start: day.Value,                          //Specified date from 00:00:00
                    end: day.Value.AddDays(1).AddSeconds(-1)); //Specified date at 25:59:59
                break;

            case RaportTimespanType.Month:
                sql = sql.Replace("/**date-range**/", "DATEPART(MONTH, Date) = @Month AND DATEPART(YEAR, Date) = DATEPART(YEAR, GETDATE())");
                dateRange = new DateRange(
                    start: new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),                          //First day of the month from specified year
                    end: new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1)); // Last day of the month from specified year
                break;

            case RaportTimespanType.Week:
                if (!weekNumber.HasValue)
                {
                    Calendar calendar = CultureInfo.CurrentCulture.Calendar;

                    weekNumber = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                }
                sql = sql.Replace("/**date-range**/", "DATEPART(wk, Date) = @WeekNumber");

                /**
                 * @HACK - this should take some customization based on user calendar settings!
                 */
                DateTime firstDayOfWeek = ISOWeek.ToDateTime(DateTime.Now.Year, weekNumber.Value, DayOfWeek.Monday);
                dateRange = new DateRange(firstDayOfWeek, firstDayOfWeek.AddDays(7));
                break;

            case RaportTimespanType.Year:
                sql = sql.Replace("/**date-range**/", "DATEPART(YEAR, Date) = @Year");
                dateRange = new DateRange(
                    new DateTime(DateTime.Now.Year, 1, 1),     //First day of the year (1st Jan )
                    new DateTime(DateTime.Now.Year, 12, 31)); //Last day of the year (31st Dec)
                break;

            case RaportTimespanType.Custom:
                sql = sql.Replace("/**date-range**/", "Date >= @StartDate AND Date <= @EndDate");
                dateRange = raportPeriod;
                break;
        }

        return dateRange;

    }

}
