using System.Data.Common;

namespace Webport.ERP.Common.Application.Database;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenPostgreSQLConnection(string? connectionString = null);

    Task<List<T>> QueryAsync<T>(string sql, object parameters = null!, bool systemDb = false);
}
