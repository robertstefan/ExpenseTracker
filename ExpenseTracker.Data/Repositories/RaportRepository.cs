using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ExpenseTracker.Core.Common.Enums;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class RaportRepository(string _connectionString) : IRaportRepository
    {
        public async Task<Raport> GenerateRaport(RaportTimespanType raportTimespanType)
        {
            TimeSpan timeSpan = ExtractTimeSpanFromRaportTimespan(raportTimespanType);


            DateTime startingDate = DateTime.UtcNow.Subtract(timeSpan);
            DateTime endingDate = DateTime.UtcNow;

            using var connection = new SqlConnection(_connectionString);

            var query = $@"
                        SELECT 
                            t.Amount,
                            t.Date,
                            c.Name AS CategoryName,
                            t.TransactionType,
                            TotalByCategory.Total
                        FROM 
                            [Transactions] t
                        INNER JOIN 
                            [Categories] c
                            ON t.CategoryId = c.Id
                        INNER JOIN 
                            (SELECT 
                                CategoryId,
                                SUM(Amount) AS Total
                             FROM 
                                [Transactions]
                             WHERE 
                                Date >= @StartingDate 
                                AND 
                                Date <= @EndingDate
                             GROUP BY 
                                CategoryId) AS TotalByCategory
                            ON t.CategoryId = TotalByCategory.CategoryId
                        WHERE
                            t.Date >= @StartingDate 
                            AND
                            t.Date <= @EndingDate";


            var result = await connection.QueryAsync(query, new
            {
                StartingDate = startingDate,
                EndingDate = endingDate
            });
            return Raport.CreateNew(1000, timeSpan, 1);
        }

        private static TimeSpan ExtractTimeSpanFromRaportTimespan(RaportTimespanType raportTimespanType)
        {
            DateTime now = DateTime.UtcNow;
            var pastDateTime = raportTimespanType switch
            {
                RaportTimespanType.Day => now.AddDays(-1),
                RaportTimespanType.Week => now.AddDays(-7),
                RaportTimespanType.Month => now.AddMonths(-1),
                RaportTimespanType.Year => now.AddYears(-1),
                _ => throw new ArgumentOutOfRangeException(nameof(raportTimespanType), raportTimespanType, null),
            };
            return now - pastDateTime;
        }
    }
}