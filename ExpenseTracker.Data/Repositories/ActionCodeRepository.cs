using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ExpenseTracker.Core.Interfaces;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Data.Repositories
{
    public class ActionCodeRepository(string _connectionString) : IActionCodeRepository
    {
        private const string TableName = "[ActionCodes]";
        public async Task<bool> AddCodeAsync(ActionCode actionCode)
        {
            var connection = new SqlConnection(_connectionString);

            string sql = @$"INSERT INTO {TableName}
                            (Code, UserId, Action, IsUsed, CreatedDateTime)
                            VALUES (@Code, @UserId, @Action, @IsUsed, GETDATE())";

            var affectedRows = await connection.ExecuteAsync(sql, actionCode);

            return affectedRows == 1;
        }

        public async Task<Guid?> UseCodeAsync(int code, int codeType)
        {
            var connection = new SqlConnection(_connectionString);

            string selectSql = @$"SELECT UserId FROM {TableName}
                          WHERE Code = @Code";

            var userId = await connection.QuerySingleOrDefaultAsync<Guid?>(selectSql, new { Code = code });

            if (userId == null)
            {
                return null;
            }

            string updateSql = @$"UPDATE {TableName}
                          SET IsUsed = 1,
                              UsedDateTime = GETDATE()
                          WHERE Code = @Code AND Action = @Action AND IsUsed = 0";

            var affectedRows = await connection.ExecuteAsync(updateSql, new { Code = code, Action = codeType });

            if (affectedRows == 1)
            {
                return userId;
            }

            return null;
        }

    }
}