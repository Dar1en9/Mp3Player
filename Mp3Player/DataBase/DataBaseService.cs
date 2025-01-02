namespace Mp3Player.DataBase;

using System.Data;
using Npgsql;
using Dapper;

public class DataBaseService
{
    private readonly string _connectionString = ConfigBuilder.AppConfigSettings.DefaultConnection;
    private async Task<IDbConnection> GetConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters? parameters = null)
    {
        using var connection = await GetConnection();
        return await connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, DynamicParameters? parameters = null)
    {
        using var connection = await GetConnection();
        return await connection.ExecuteAsync(sql, parameters);
    }
}

