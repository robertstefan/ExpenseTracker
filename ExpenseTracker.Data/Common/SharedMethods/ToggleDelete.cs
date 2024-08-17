using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace ExpenseTracker.Data.Common.SharedMethods;

public static class ToggleDelete
{
    public async static Task<int> Handle(Guid id, string TableName, SqlConnection conn, bool SoftDelete = true)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@Id", id, DbType.Guid);

        parameters.Add("@TableName", TableName, DbType.String);

        parameters.Add("@IsSoft", SoftDelete, DbType.Boolean);

        return await conn.ExecuteAsync("ToggleDelete", parameters);
    }
}
