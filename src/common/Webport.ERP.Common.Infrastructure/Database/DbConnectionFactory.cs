using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;
using Webport.ERP.Common.Application.Database;

namespace Webport.ERP.Common.Infrastructure.Database;

internal sealed class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenPostgreSQLConnection(string? connectionString = null)
    {
        NpgsqlConnection connection = new(configuration["PostgreSQL:TenantConnection"]);

        await connection.OpenAsync();

        return connection;
    }

    public async Task<List<T>> QueryAsync<T>(string sql, object parameters = null!, bool systemDb = false)
    {
        using DbConnection connection = await OpenPostgreSQLConnection();
        //await connection.OpenAsync();

        IEnumerable<T> result = await connection.QueryAsync<T>(sql, parameters);
        return [.. result];
    }
}