using Dapper;

namespace Mp3Player.DataBase;

public interface IDataBaseService
{
    Task<IEnumerable<T>> QueryAsync<T>(string sql, DynamicParameters? parameters = null);
    Task<int> ExecuteAsync(string sql, DynamicParameters? parameters = null);

}