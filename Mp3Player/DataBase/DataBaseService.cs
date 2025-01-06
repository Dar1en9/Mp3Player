namespace Mp3Player.DataBase;

using System.Data;
using Npgsql;
using Dapper;

public class DataBaseService: IDataBaseService
{
    private readonly IDbConnection _connection;

    public DataBaseService(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters? parameters = null)
    {
        return await _connection.QueryAsync<T>(sql, parameters);
    }

    public async Task<int> ExecuteAsync(string sql, DynamicParameters? parameters = null)
    {
        return await _connection.ExecuteAsync(sql, parameters);
    }
}

