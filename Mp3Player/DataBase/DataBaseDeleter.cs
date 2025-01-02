using Dapper;
using Microsoft.Extensions.Logging;

namespace Mp3Player.DataBase;

public class DataBaseDeleter: IDataBaseDeleter { 
    private readonly DataBaseService _dbService;
    private readonly ILogger _logger;
    
    public DataBaseDeleter(DataBaseService dbService, ILogger logger) {
        _dbService = dbService;
        _logger = logger;
    }
    public async Task<bool> DeleteTrack(string id)
    {
        _logger.LogDebug("Удаление трека из базы данных по ID: {TrackId}", id);
        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        var rowsDeleted = await _dbService.ExecuteAsync("DELETE FROM Tracks WHERE Id = @Id", parameters);
        if (rowsDeleted == 0)
        {
            _logger.LogWarning("Трека с ID {TrackId} нет в базе данных", id);
            return false;
        }
        _logger.LogDebug("Трек с ID {TrackId} удален из базы данных", id);
        return true;
    }
}