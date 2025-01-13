using System.Data;
using System.Data.SQLite;

namespace PastPaperRepository.Application.Database;

public class SqliteConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new SQLiteConnection(_connectionString);
        await connection.OpenAsync(token);
        return connection;
    }
}